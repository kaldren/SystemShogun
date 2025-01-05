variable "resource_group_name" {
  description = "The name of the resource group in which to create the resources."
  type = string
  default = "rg-moviescosmos"
}

variable "location" {
  description = "The Azure region in which to create the resources."
  type = string
  default = "West Europe"
}

variable "app_service_plan_name" {
  description = "The name of the App Service Plan."
  type = string
  default = "asp-moviesapp-cosmosdb"
}

variable "web_api_app_name" {
  description = "The name of the Web API App Service."
  type = string
  default = "app-moviesapp-cosmosdb"
}

variable "blazor_web_app_name" {
  description = "The name of the Blazor Web App Service."
  type = string
  default = "app-moviesapp-cosmosdb-blazor"
}

variable "cosmos_db_name" {
  description = "The name of the Cosmos DB account."
  type = string
  default = "moviesappcosmosdb"
}

variable "sku" {
  description = "The SKU of the App Service Plan."
  type = string
  default = "B1" # Free tier for App Service Plan
}

variable "cosmos_db_offer_type" {
  description = "The offer type for the Cosmos DB account."
  type = string
  default = "Standard"
}

variable "subscription_id" {
  description = "The Azure subscription ID."
  type = string
  default = "[ADD YOUR SUBSCRIPTION ID HERE]"
}

variable "keyvault_moviescosmos1" {
  description = "The name of the Key Vault."
  type = string
  default = "kv-moviescosmos01"
}

variable "keyvault_moviescosmos1_dbsecret" {
  description = "The name of the Key Vault secret for the Cosmos DB connection string."
  type = string
  default = "MoviesDbConnection"
}


variable "uami_github" {
  description = "The User Assigned Managed Identity for GitHub Actions."
  type = string
  default = "uami_github"
}