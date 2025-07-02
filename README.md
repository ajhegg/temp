
# AI Chat with Custom Data (Customized)

This project is an AI chat application that lets you chat with your own data using Azure OpenAI, Azure AI Search, and Azure Blob Storage. It is based on a .NET Aspire template, but has been customized for:

- Integration with your own Azure AI Search index and endpoint
- Integration with your own Azure Blob Storage for PDF viewing
- Custom configuration and package setup

---

## Prerequisites

- .NET 9.0 SDK
- Azure account with OpenAI, AI Search, and Blob Storage resources

---

## Configuration

### Azure OpenAI
1. Create an Azure OpenAI Service resource and deploy the `gpt-4.1` and `text-embedding-3-small` models.
2. Use .NET User Secrets to store your API key and endpoint (do not put secrets in source control):
   ```sh
   cd temp.AppHost
   dotnet user-secrets set ConnectionStrings:openai "Endpoint=https://YOUR-DEPLOYMENT-NAME.openai.azure.com;Key=YOUR-API-KEY"
   ```

### Azure AI Search
1. Create an Azure AI Search resource and index (e.g., `ai-search-1751136271295`).
2. Store your endpoint and admin key using .NET User Secrets:
   ```sh
   cd temp.AppHost
   dotnet user-secrets set ConnectionStrings:azureAISearch "Endpoint=https://YOUR-DEPLOYMENT-NAME.search.windows.net;Key=YOUR-API-KEY"
   ```

### Azure Blob Storage
1. Set your public Blob Storage base URL in `temp.Web/appsettings.json`:
   ```json
   "BlobBaseUrl": "https://abigailhegg8126storage.blob.core.windows.net/abigailhegg8126blobcontainer/"
   ```
   If your blobs are private, you will need to generate SAS tokens and update the code to append them to the URLs.

---

## Required NuGet Packages

Install these in the `temp.Web` project:

```sh
dotnet add package Aspire.Azure.AI.OpenAI --version 9.3.0-preview.1.25265.20
dotnet add package Azure.Search.Documents --version 11.6.1
dotnet add package Microsoft.Extensions.AI.OpenAI --version 9.5.0-preview.1.25265.7
dotnet add package Microsoft.Extensions.AI --version 9.5.0
dotnet add package Microsoft.SemanticKernel.Core --version 1.53.0
dotnet add package PdfPig --version 0.1.10
dotnet add package System.Linq.Async --version 6.0.1
dotnet add package Aspire.Azure.Search.Documents --version 9.3.0
dotnet add package Microsoft.SemanticKernel.Connectors.AzureAISearch --version 1.53.0-preview
```

---

## Running the Application

### Visual Studio
1. Open the `.sln` file.
2. Press `Ctrl+F5` or click "Start" to run.

### Visual Studio Code
1. Open the project folder.
2. Install the [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit).
3. Open `Program.cs` in `temp.AppHost` and click "Run" in the Debug view.

---

## Notes on Customization

- The app uses your Azure AI Search index and endpoint, as set in `appsettings.json` and user secrets.
- PDF files are served from your Azure Blob Storage. The viewer URL is constructed in `ChatCitation.razor` using the `BlobBaseUrl`.
- No sensitive information is stored in source control; all secrets are managed via .NET User Secrets.

---

## Updating JavaScript Dependencies

JavaScript libraries are in `wwwroot/lib` of the `temp.Web` project. See the README in each subfolder for update instructions.

---

## Learn More

- [.NET AI documentation](https://learn.microsoft.com/dotnet/ai/)
- [Azure OpenAI Service](https://learn.microsoft.com/azure/ai-services/openai/)
- [Azure AI Search](https://learn.microsoft.com/azure/search/)

