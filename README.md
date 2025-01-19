# SystemShogun

Welcome to **SystemShogun**! This repository is a collection of examples and resources to complement the blog posts that I write on my blog [SystemShogun.com](https://systemshogun.com/). Here, you’ll find practical, real-world implementations of concepts I cover in there, mainly related to [.NET](https://dot.net/) and [Microsoft Azure](https://azure.com/).

## Purpose

The goal of this repository is to:
- Provide **working examples** that you can clone, run, and customize.
- Demonstrate **best practices** for cloud development, automation, and secure deployments.
- Serve as a companion to my blog posts, where I dive deeper into the "why" and "how."

## Who Is This For?

This repository is for:
- **Developers** looking for practical examples of cloud development.
- **Architects** interested in designing scalable and secure infrastructure.
- **DevOps Engineers** who want to automate cloud deployments.
- **Learners** eager to understand modern tools like Terraform, GitHub Actions, and Azure.

## Blog content

#### Deploy from GitHub Actions to Function App behind private endpoints (or completely disabled for public access)
This branch provides a step-by-step guide on how to deploy your application to an Azure Function App that is behind private endpoints or completely disabled for public access. It includes configurations and best practices for setting up private access while maintaining secure and efficient deployment processes.

#### Key Features:
- Deploying Function Apps with private endpoints or completely restricted public access
- Restricting public access to Azure Function Apps
- Configuring deployment using GitHub Actions

For a detailed guide, check out the blog post: [GitHub Actions and Private Endpoints](https://systemshogun.com/p/github-actions-and-private-endpoints)

#### Use Federated Identity in Microsoft Azure to Authenticate with GitHub Actions
This branch provides a step-by-step guide on how to deploy your application using Federated Identity, eliminating the need for storing credentials or secrets in your CI/CD pipeline.

#### Key Features:
- Deploying to Azure using Federated Identity integration
- Setting up GitHub Actions to authenticate securely with Azure
- Using Terraform to automate infrastructure deployment
- Improving security by removing the need for storing credentials

For a detailed guide, check out the blog post: [How I Deployed My App to Azure with GitHub Actions, Terraform, and NO CREDENTIALS](https://systemshogun.com/p/how-i-deployed-my-app-to-azure-with)

## How to Use This Repository

1. **Clone the Repo:**
   ```bash
   git clone https://github.com/kaldren/SystemShogun.git
   cd SystemShogun
   ```

2. **Explore Examples:**
   Each folder is organized by topic or project, so you can easily find the example you're looking for.

3. **Follow the Blog Posts:**
   For detailed explanations, follow along with the blog posts linked in the `README` of each project folder.

4. **Customize and Experiment:**
   Modify the examples to fit your use case or experiment with different configurations.

## Contributions

Feel free to:
- Open issues for any questions or improvements.
- Submit pull requests with additional examples or enhancements.
- Share your feedback to help improve the repository.

## License

This repository is licensed under the **MIT License**. You are free to use, modify, and distribute the code, but attribution is appreciated.

## Contact

If you have any questions or just want to connect:
- Visit my blog: [SystemShogun Blog](https://systemshogun.com/)
- Say HI on [LinkedIn](https://linkedin.com/in/kdrenski/)
- Email: [your-email@example.com](mailto:kaloyandrenski@gmail.com)
