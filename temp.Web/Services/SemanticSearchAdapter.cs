namespace temp.Web.Services;

public class SemanticSearchAdapter(ExistingIndexSemanticSearch existingSearch) : SemanticSearch(null!)
{
    public override async Task<IReadOnlyList<IngestedChunk>> SearchAsync(string text, string? documentIdFilter, int maxResults)
    {
        return await existingSearch.SearchAsync(text, documentIdFilter, maxResults);
    }
}
