using Microsoft.Extensions.AI;
using temp.Web.Services;
using temp.Web.Services.Ingestion;
using temp.Web.Components;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var openai = builder.AddAzureOpenAIClient("openai");
openai.AddChatClient("gpt-4.1")
    .UseFunctionInvocation()
    .UseOpenTelemetry(configure: c =>
        c.EnableSensitiveData = builder.Environment.IsDevelopment());
openai.AddEmbeddingGenerator("text-embedding-3-small");

builder.AddAzureSearchClient("azureAISearch");
builder.Services.AddAzureAISearchCollection<ExistingIndexChunk>("ai-search-1751136271295");
builder.Services.AddAzureAISearchCollection<IngestedDocument>("data-temp-documents");
// builder.Services.AddScoped<DataIngestor>(); // Commented out since using existing index
builder.Services.AddSingleton<SemanticSearch, ExistingIndexSemanticSearch>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.UseStaticFiles();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Commented out PDF ingestion since using existing Azure AI Search index
// By default, we ingest PDF files from the /wwwroot/Data directory. You can ingest from
// other sources by implementing IIngestionSource.
// Important: ensure that any content you ingest is trusted, as it may be reflected back
// to users or could be a source of prompt injection risk.
// await DataIngestor.IngestDataAsync(
//     app.Services,
//     new PDFDirectorySource(Path.Combine(builder.Environment.WebRootPath, "Data")));

app.Run();
