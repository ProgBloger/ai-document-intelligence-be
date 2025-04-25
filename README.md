# AI Document Intelligence
Building AI Document Intelligence with .NET and Azure

## Prerequisites

You have to have these tools installed on your machine:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

Additionally, prepare the following settings and extensions:

### Docker Desktop

Enable Kubernetes for the Docker Desktop:

![Docker Desktop enabling Kubernetes](kube-screen-shot.png)

### Visual Studio Code

Install the following extensions:

- [YAML 1.11.10112022](https://marketplace.visualstudio.com/items?itemName=redhat.vscode-yaml)
- [C# 1.25.0](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
- [dotnet 1.3.0](https://marketplace.visualstudio.com/items?itemName=leo-labs.dotnet)
- [NuGet Package Manager 1.1.6](https://marketplace.visualstudio.com/items?itemName=jmrog.vscode-nuget-package-manager)
- [Azure Account 0.11.2](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azure-account)
- [Azure CLI Tools 0.5.0](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azurecli)
- [Docker 1.22.1](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-docker)
- [Kubernetes 1.3.10](https://marketplace.visualstudio.com/items?itemName=ms-kubernetes-tools.vscode-kubernetes-tools)
- [Kubernetes Support 0.1.9](https://marketplace.visualstudio.com/items?itemName=ipedrazas.kubernetes-snippets)
- [Azure Kubernetes Service](https://marketplace.visualstudio.com/items?itemName=ms-kubernetes-tools.vscode-aks-tools)

## Workshop

This guide helps you to build a Cloud Native Application that consists of a number of services written in .NET 8.0 using C#

- [Step 01](steps/step-01/README.md) - Create the Resource Group
- [Step 02](steps/step-02/README.md) - Set Up Azure Key Vault
- [Step 03](steps/step-03/README.md) - Create Storage Account with Azure Blob Storage
- [Step 04](steps/step-04/README.md) - Deploy the Backend API (with Blob Storage & Key Vault integration)
- [Step 05](steps/step-05/README.md) - Deploy the Frontend UI (with Key Vault integration)
- [Step 06](steps/step-06/README.md) - Create and Deploy Azure Function
- [Step 07](steps/step-07/README.md) - Connect Azure Function to Blob Storage (with Key Vault)
- [Step 08](steps/step-08/README.md) - Set Up Azure AI Form Recognizer
- [Step 09](steps/step-09/README.md) - Save Parsed Data to Blob Storage
- [Step 10](steps/step-10/README.md) - Load Parsed Data in Backend (with Key Vault)
- [Step 11](steps/step-11/README.md) - Implement the Editing Interface in Backend
- [Step 12](steps/step-12/README.md) - Set Up Azure OpenAI GPT Model
- [Step 13](steps/step-13/README.md) - Integrate GPT Model with Backend (with Key Vault)
- [Step 14](steps/step-14/README.md) - Save Verified Data (Stub DB)
- [Step 15](steps/step-15/README.md) - Create Azure Cosmos DB with Embeddings
- [Step 16](steps/step-16/README.md) - Set Up Embeddings Model
- [Step 17](steps/step-17/README.md) - Store Embeddings in Cosmos DB (with Key Vault)
- [Step 18](steps/step-18/README.md) - Process GPT Responses in Backend

