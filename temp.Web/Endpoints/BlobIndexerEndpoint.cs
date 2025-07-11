using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace temp.Web.Endpoints
{
    [ApiController]
    [Route("api/blob-index")] // POST /api/blob-index
    public class BlobIndexerEndpoint : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        public BlobIndexerEndpoint(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> RunIndexer()
        {
            var endpoint = _config["AzureSearch:Endpoint"];
            var indexerName = _config["AzureSearch:IndexerName"] ?? "ai-search-new-indexer";
            var apiKey = _config["AzureSearch:ApiKey"];
            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(apiKey))
                return BadRequest("Azure Search endpoint or API key not configured.");

            var url = $"{endpoint}/indexers/{indexerName}/run?api-version=2023-10-01-Preview";
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("api-key", apiKey);
            var response = await client.PostAsync(url, null);
            if (response.IsSuccessStatusCode)
                return Ok();
            var error = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, error);
        }
    }
}
