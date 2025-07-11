using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace temp.Web.Endpoints
{
    [ApiController]
    [Route("api/blob-sas")]
    public class BlobSasEndpoint : ControllerBase
    {
        private readonly IConfiguration _config;
        public BlobSasEndpoint(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult GetSasUrl([FromBody] SasRequest request)
        {
            var blobBaseUrl = _config["BlobBaseUrl"];
            var connectionString = _config.GetConnectionString("AzureBlob") ?? _config["AzureBlobConnectionString"];
            if (string.IsNullOrEmpty(blobBaseUrl) || string.IsNullOrEmpty(connectionString))
                return BadRequest("BlobBaseUrl or connection string not configured.");

            var containerName = new Uri(blobBaseUrl).Segments[1].TrimEnd('/');
            var blobName = request.FileName;
            var blobClient = new BlobContainerClient(connectionString, containerName).GetBlobClient(blobName);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Write | BlobSasPermissions.Create);

            var sasUri = blobClient.GenerateSasUri(sasBuilder);
            return Ok(new { sasUrl = sasUri.ToString() });
        }

        public class SasRequest
        {
            public string FileName { get; set; } = string.Empty;
        }
    }
}
