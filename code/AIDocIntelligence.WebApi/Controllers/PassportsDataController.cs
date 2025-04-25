using Microsoft.AspNetCore.Mvc;
using AIDocIntelligence.WebApi.Services;
using AIDocIntelligence.WebApi.ViewModels;
using Azure.Data.Tables;
using Microsoft.Azure.Cosmos;

namespace AIDocIntelligence.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassportsDataController : ControllerBase
    {
        private readonly TableClient _tableClient;
        private readonly OpenAIService _openAIService;
        private readonly Container _cosmosDbContainer;
        private readonly ILogger<PassportsDataController> _logger;

        public PassportsDataController(
            Func<string, TableClient> tableClientFactory,
            OpenAIService openAIService,
            Container cosmosDbContainer,
            ILogger<PassportsDataController> logger)
        {
            _tableClient = tableClientFactory("PassportData");
            _openAIService = openAIService;
            _cosmosDbContainer = cosmosDbContainer;
            _logger = logger;
        }

        [HttpGet("{partitionKey}/{rowKey}", Name = "GetPassportData")]
        public ActionResult<List<CorrectedProperty>> Get(string partitionKey, string rowKey)
        {
            var resultList = new List<CorrectedProperty>();

            try
            {
                var query = _tableClient.Query<TablePassportData>(it => it.PartitionKey == partitionKey && it.RowKey == rowKey);

                var entity = query.FirstOrDefault();

                if (entity == null)
                {
                    return Ok(resultList);
                }

                var properties = typeof(TablePassportData).GetProperties();

                foreach (var property in properties)
                {
                    string propertyName = property.Name;
                    object value = property.GetValue(entity) ?? string.Empty;
                    if(value is null)
                    {
                        continue;
                    }

                    resultList.Add(new CorrectedProperty
                    {
                        PropertyName = propertyName,
                        SuggestedValue = value.ToString()!,
                        IsValid = null
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return Ok(resultList);
        }

        [HttpPost(Name = "ValidatePassportData")]
        public async Task<ActionResult<List<CorrectedProperty>>> PostAsync([FromBody] PassportDataToVerify passportData, CancellationToken cancellationToken)
        {
            List<CorrectedProperty> result = default!;
            try
            {
                result = await _openAIService.ValidatePassportData(passportData, cancellationToken);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return Ok(result);
        }

        [HttpPut(Name = "SavePassportData")]
        public async Task<ActionResult<List<CorrectedProperty>>> PutAsync([FromBody] PassportData passportData, CancellationToken cancellationToken)
        {
            try
            {
                var searchPassData = new SearchPassportData
                {
                    RowKey = passportData.RowKey,
                    PartitionKey = passportData.PartitionKey,
                    FirstName = passportData.FirstName,
                    LastName = passportData.LastName,
                    Nationality = passportData.Nationality,
                    DocumentType = passportData.DocumentType,
                    DocumentNumber = passportData.DocumentNumber,
                    IssuingAuthority = passportData.IssuingAuthority,
                    MachineReadableZone = passportData.MachineReadableZone,
                    PlaceOfBirth = passportData.PlaceOfBirth,
                    Sex = passportData.Sex,

                    DateOfIssue = DateTime.SpecifyKind(passportData.DateOfIssue, DateTimeKind.Utc),
                    DateOfBirth = DateTime.SpecifyKind(passportData.DateOfBirth, DateTimeKind.Utc),
                    DateOfExpiration = DateTime.SpecifyKind(passportData.DateOfExpiration, DateTimeKind.Utc),
                };

                searchPassData.Vectors = await _openAIService.GetEmbeddingsAsync(passportData, cancellationToken);

                // Store in CosmosDB
                await _cosmosDbContainer.UpsertItemAsync(
                    searchPassData,
                    new PartitionKey(searchPassData.DocumentNumber)
                );

                _logger.LogInformation("Passport Data saved to Table Storage.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return NoContent();
        }
    }
}
