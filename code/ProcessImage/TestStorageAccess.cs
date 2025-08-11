using System.Net;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace ProcessImage
{
    public class TestRealConnectionBlobTrigger
    {
        private readonly ILogger<TestRealConnectionBlobTrigger> _logger;

        public TestRealConnectionBlobTrigger(ILogger<TestRealConnectionBlobTrigger> logger)
        {
            _logger = logger;
        }

        [Function("TestRealConnectionBlobTrigger")]
        public void Run(
            [BlobTrigger("passports/{name}", Connection = "realStorageConString")] Stream blobStream, string name)
        {

            _logger.LogInformation($"Blob trigger fired for: {name}");
            _logger.LogInformation($"Blob length: {blobStream.Length} bytes");
        }
    }
}
