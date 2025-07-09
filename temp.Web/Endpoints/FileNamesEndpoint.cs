using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using temp.Web.Services;

namespace temp.Web.Endpoints;

public static class FileNamesEndpoint
{
    public static void MapFileNamesEndpoint(this WebApplication app)
    {
        app.MapGet("/api/filenames", async (SemanticSearch search) =>
        {
            // Get all unique file names (DocumentId = title in ExistingIndexChunk)
            var results = await search.SearchAsync("*", null, 1000);
            var fileNames = results.Select(r => r.DocumentId).Distinct().ToList();
            return Results.Ok(fileNames);
        });
    }
}
