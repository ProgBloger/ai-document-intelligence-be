using AIDocIntelligence.WebApi.Services;
using AIDocIntelligence.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace AIDocIntelligence.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassportsSearchController : ControllerBase
    {
        private readonly OpenAIService _openAIService;
        private readonly Container _cosmosDbContainer;

        public PassportsSearchController(
            OpenAIService openAIService,
            Container cosmosDbContainer)
        {
            _openAIService = openAIService;
            _cosmosDbContainer = cosmosDbContainer;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync([FromQuery] string userQuery, CancellationToken cancellationToken)
        {
            var embeddingVector = await _openAIService.GetEmbeddingsAsync(userQuery, cancellationToken);

            var retrievedDocs = await SingleVectorSearchAsync(embeddingVector, cancellationToken);

            foreach (var document in retrievedDocs)
            {
                document.Vectors = null!; // removing embedding to reduce tokens during chat completion
            }

            var completion = await _openAIService.GetChatCompletionAsync(userQuery, JsonConvert.SerializeObject(retrievedDocs), cancellationToken);

            return Ok(completion);
        }

        private async Task<List<SearchPassportData>> SingleVectorSearchAsync(float[] vectors, CancellationToken cancellationToken)
        {
            const double similarityScore = 0.30;
            string queryText = @"SELECT TOP 1 x.FirstName, x.LastName, x.DateOfBirth, x.DateOfIssue, x.DateOfExpiration, 
                         x.Nationality, x.DocumentType, x.DocumentNumber, x.IssuingAuthority, 
                         x.MachineReadableZone, x.PlaceOfBirth, x.Sex, x.SimilarityScore
                  FROM (
                      SELECT c.FirstName, c.LastName, c.DateOfBirth, c.DateOfIssue, c.DateOfExpiration, 
                             c.Nationality, c.DocumentType, c.DocumentNumber, c.IssuingAuthority, 
                             c.MachineReadableZone, c.PlaceOfBirth, c.Sex, 
                             VectorDistance(c.Vectors, @vectors) AS SimilarityScore
                      FROM c
                  ) x
                  WHERE x.SimilarityScore > @similarityScore
                  ORDER BY x.SimilarityScore DESC";

            var queryDef = new QueryDefinition(
                    query: queryText)
                .WithParameter("@vectors", vectors)
                .WithParameter("@similarityScore", similarityScore);

            using FeedIterator<SearchPassportData> resultSet = _cosmosDbContainer.GetItemQueryIterator<SearchPassportData>(queryDefinition: queryDef);

            var documents = new List<SearchPassportData>();

            while (resultSet.HasMoreResults)
            {
                FeedResponse<SearchPassportData> response = await resultSet.ReadNextAsync(cancellationToken);
                documents.AddRange(response);
            }

            return documents;
        }
    }
}
