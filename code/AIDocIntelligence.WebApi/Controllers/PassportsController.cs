using Azure.Data.Tables;
using AIDocIntelligence.WebApi.ViewModels;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace AIDocIntelligence.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassportsController : ControllerBase
    {
        private readonly TableClient _tableClient;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ILogger<PassportsController> _logger;

        public PassportsController(
            Func<string, TableClient> tableClientFactory,
            BlobContainerClient blobContainerClient,
            ILogger<PassportsController> logger)
        {
            _tableClient = tableClientFactory("OcrResults");
            _blobContainerClient = blobContainerClient;
            _logger = logger;
        }

        [HttpGet(Name = "GetPassports")]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _tableClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

                var queryResults = _tableClient.QueryAsync<OcrPassportData>(filter: string.Empty, cancellationToken: cancellationToken);

                var entities = new List<dynamic>();

                await foreach (var passport in queryResults.WithCancellation(cancellationToken))
                {
                    entities.Add(new { Id = passport.RowKey, FirstName = passport.FirstName, LastName = passport.LastName });
                }

                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex!.Message);
                throw;
            }
        }

        [HttpGet("{id:guid}", Name = "GetPassport")]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _tableClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

                var queryResults = _tableClient.QueryAsync<OcrPassportData>(filter: $"RowKey eq '{id}'", cancellationToken: cancellationToken);
                
                var enumerator = queryResults.GetAsyncEnumerator(cancellationToken);
                if (await enumerator.MoveNextAsync())
                {
                    var passportData = enumerator.Current;

                    return Ok(passportData);
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }

            return NotFound();
        }

        [HttpPost(Name = "PostPassport")]
        public async Task<IActionResult> PostAsync(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0) 
            {
                return BadRequest("No File Uploaded");
            }

            try
            {
                await _blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

                var blobClient = _blobContainerClient.GetBlobClient(file.FileName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true, cancellationToken);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading file: {ex.Message}");
            }
        }
    }
}
