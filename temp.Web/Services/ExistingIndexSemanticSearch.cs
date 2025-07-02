using Microsoft.Extensions.VectorData;

namespace temp.Web.Services;

public class ExistingIndexSemanticSearch : SemanticSearch
{
    private readonly VectorStoreCollection<string, ExistingIndexChunk> _vectorCollection;

    public ExistingIndexSemanticSearch(VectorStoreCollection<string, ExistingIndexChunk> vectorCollection) 
        : base(null!) // Pass null to base since we're overriding the method
    {
        _vectorCollection = vectorCollection;
    }

    public override async Task<IReadOnlyList<IngestedChunk>> SearchAsync(string text, string? documentIdFilter, int maxResults)
    {
        var nearest = _vectorCollection.SearchAsync(text, maxResults, new VectorSearchOptions<ExistingIndexChunk>
        {
            Filter = documentIdFilter is { Length: > 0 } ? 
                record => record.title == documentIdFilter || 
                         record.metadata_storage_path == documentIdFilter || 
                         record.parent_id == documentIdFilter : null,
        });

        var results = await nearest.Select(result => result.Record).ToListAsync();
        
        // Convert to IngestedChunk format
        return results.Select(chunk => new IngestedChunk
        {
            Key = chunk.Key,
            DocumentId = chunk.DocumentId,
            PageNumber = chunk.PageNumber,
            Text = chunk.Text
        }).ToList();
    }
}
