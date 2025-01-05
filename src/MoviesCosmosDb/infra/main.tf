# Provider Configuration
provider "azurerm" {
  subscription_id = var.subscription_id
  features {}
}

data "azurerm_client_config" "current" {}

# Resource Group
resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

# App Service Plan
resource "azurerm_service_plan" "app_service_plan" {
  name                = var.app_service_plan_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = var.sku
  depends_on          = [azurerm_resource_group.rg]
}

# Web API App Service
resource "azurerm_linux_web_app" "web_api" {
  name                = var.web_api_app_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = azurerm_service_plan.app_service_plan.id

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.uami_github.id]
  }

  key_vault_reference_identity_id = azurerm_user_assigned_identity.uami_github.id

  app_settings = {
    "CosmosDbConnectionString" = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.cosmos_connection_string.id})"
    "KeyVaultUrl"              = azurerm_key_vault.kv.vault_uri
    "AZURE_CLIENT_ID"          = azurerm_user_assigned_identity.uami_github.client_id
  }

  site_config {
    application_stack {
      dotnet_version = "9.0"
    }
  }
  depends_on = [azurerm_service_plan.app_service_plan]
}

# Blazor Web App Service
resource "azurerm_linux_web_app" "blazor_web" {
  name                = var.blazor_web_app_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = azurerm_service_plan.app_service_plan.id

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.uami_github.id]
  }

  app_settings = {
    "KeyVaultUrl" = azurerm_key_vault.kv.vault_uri
  }

  site_config {
    application_stack {
      dotnet_version = "9.0"
    }
  }
  depends_on = [azurerm_service_plan.app_service_plan]
}

# Cosmos DB Account
resource "azurerm_cosmosdb_account" "cosmos_db" {
  name                = var.cosmos_db_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  offer_type          = var.cosmos_db_offer_type
  kind                = "GlobalDocumentDB"
  consistency_policy {
    consistency_level = "Session"
  }

  geo_location {
    location          = azurerm_resource_group.rg.location
    failover_priority = 0
  }

  depends_on = [azurerm_service_plan.app_service_plan]
}

# User Assigned Managed Identity
resource "azurerm_user_assigned_identity" "uami_github" {
  name                = var.uami_github
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  depends_on = [azurerm_cosmosdb_account.cosmos_db]
}

# Federated Credential for GitHub Actions
resource "azurerm_federated_identity_credential" "uami_github_credential" {
  name                 = "uami-github-githubactions"
  resource_group_name  = azurerm_resource_group.rg.name
  audience             = ["api://AzureADTokenExchange"]
  issuer               = "https://token.actions.githubusercontent.com"
  parent_id			   = azurerm_user_assigned_identity.uami_github.id
  subject              = "repo:[YOUR GITHUB USER]/[YOUR REPO]:ref:refs/heads/main"
}

# Assign Contributor Role for the Resource Group to the Managed Identity
resource "azurerm_role_assignment" "uami_contributor" {
  scope                = azurerm_resource_group.rg.id
  role_definition_name = "Contributor"
  principal_id         = azurerm_user_assigned_identity.uami_github.principal_id
}

# Key Vault
resource "azurerm_key_vault" "kv" {
  name                = var.keyvault_moviescosmos1
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku_name            = "standard"

  tenant_id           = data.azurerm_client_config.current.tenant_id

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = azurerm_user_assigned_identity.uami_github.principal_id

    secret_permissions = [
      "Get",
      "List",
      "Set",
      "Delete"
    ]
  }

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    secret_permissions = [
      "Get",
      "List",
      "Set",
      "Delete"
    ]
  }

  depends_on = [azurerm_cosmosdb_account.cosmos_db]
}

# Key Vault Secret
resource "azurerm_key_vault_secret" "cosmos_connection_string" {
  name         = var.keyvault_moviescosmos1_dbsecret
  value        = azurerm_cosmosdb_account.cosmos_db.primary_sql_connection_string
  key_vault_id = azurerm_key_vault.kv.id
  depends_on = [azurerm_cosmosdb_account.cosmos_db]
}

# Outputs
output "resource_group_name" {
  value = azurerm_resource_group.rg.name
}

output "web_api_app_url" {
  value = azurerm_linux_web_app.web_api.default_hostname
}

output "blazor_web_app_url" {
  value = azurerm_linux_web_app.blazor_web.default_hostname
}

output "cosmos_db_name" {
  value = azurerm_cosmosdb_account.cosmos_db.name
}

output "uami_github_client_id" {
  value = azurerm_user_assigned_identity.uami_github.client_id
}
