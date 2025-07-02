using Microsoft.Extensions.VectorData;

namespace temp.Web.Services;

public class ExistingIndexChunk
{
    private const int VectorDimensions = 1536; // Matches your index vector dimensions
    private const string VectorDistanceFunction = DistanceFunction.CosineSimilarity;

    [VectorStoreKey]
    public string chunk_id { get; set; } = string.Empty;

    [VectorStoreData(IsIndexed = true)]
    public string parent_id { get; set; } = string.Empty;

    [VectorStoreData(IsIndexed = true)]
    public string chunk { get; set; } = string.Empty;

    [VectorStoreData(IsIndexed = true)]
    public string title { get; set; } = string.Empty;

    [VectorStoreData]
    public string metadata_storage_path { get; set; } = string.Empty;

    [VectorStoreVector(VectorDimensions, DistanceFunction = VectorDistanceFunction)]
    public ReadOnlyMemory<float>? text_vector { get; set; }

    // Properties to map to the expected IngestedChunk interface
    public string Key => chunk_id;
    public string DocumentId => title;
    public int PageNumber => 1; // Default value since your index doesn't have page numbers
    public string Text => chunk;
}
