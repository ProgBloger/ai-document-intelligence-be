using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure;
using Microsoft.Azure.Functions.Worker;
using System.Text;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ProcessImage.Common;

namespace ProcessImage
{
    public class FormRecognizerActivity
    {
        private readonly SecretProvider _secretProvider;

        public FormRecognizerActivity(SecretProvider secretProvider)
        {
            _secretProvider = secretProvider;
        }

        [Function(nameof(ProcessWithFormRecognizer))]
        public async Task<OcrResultEntity> ProcessWithFormRecognizer(
            [ActivityTrigger] string blobName, 
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger(nameof(ProcessWithFormRecognizer));

            try
            {
                logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {blobName}");
                
                string storageConnStr = await _secretProvider.GetSecretAsync("AzureStorageConnectionString");

                var blobServiceClient = new BlobServiceClient(storageConnStr);
                var containerClient = blobServiceClient.GetBlobContainerClient(ResourceNames.PassportsContainerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                BlobDownloadInfo blobDownload = await blobClient.DownloadAsync();

                using MemoryStream seekableStream = new MemoryStream();
                await blobDownload.Content.CopyToAsync(seekableStream);

                seekableStream.Position = 0;
                
                var endpoint = await _secretProvider.GetSecretAsync("FormRecognizerEndpoint");
                var apiKey = await _secretProvider.GetSecretAsync("FormRecognizerKey");

                var client = new DocumentAnalysisClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

                var operation = await client.AnalyzeDocumentAsync(
                    waitUntil: WaitUntil.Started,
                    modelId: "prebuilt-idDocument",
                    document: seekableStream,
                    options: null,
                    cancellationToken: executionContext.CancellationToken
                );

                var result = await operation.WaitForCompletionAsync();

                var entity = new OcrResultEntity
                {
                    PartitionKey = "DefaultKey",
                    RowKey = Guid.NewGuid().ToString(),
                    DocumentName = blobName,
                    Timestamp = DateTimeOffset.UtcNow
                };

                var sb = new StringBuilder();

                var sbValues = new StringBuilder();

                PopulateTheEntity(result, entity, sb, sbValues, executionContext);

                entity.OcrResult = sb.ToString();
                entity.OcrValues = sbValues.ToString();

                logger.LogInformation("Form Recognizer processing complete.");

                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception caught in function: {ex.Message}");
                logger.LogError(ex.ToString());

                throw;
            }
        }

        private static void PopulateTheEntity(
            Response<AnalyzeResult> result, 
            OcrResultEntity entity, 
            StringBuilder sb, 
            StringBuilder sbValues,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger(nameof(ProcessWithFormRecognizer));

            foreach (var document in result.Value.Documents)
            {
                foreach (var field in document.Fields)
                {
                    logger.LogInformation($"Awesome Field '{field.Key}': '{field.Value.Content}'");

                    sb.Append($"'{field.Key}': '{field.Value.Content}'\n");
                    sbValues.Append(field.Value.Content);
                    sbValues.Append(Environment.NewLine);

                    switch (field.Key)
                    {
                        case "FirstName":
                            entity.FirstName = field.Value.Content;
                            break;
                        case "LastName":
                            entity.LastName = field.Value.Content;
                            break;
                        case "DateOfBirth":
                            entity.DateOfBirth = field.Value.Content;
                            break;
                        case "DateOfIssue":
                            entity.DateOfIssue = field.Value.Content;
                            break;
                        case "DateOfExpiration":
                            entity.DateOfExpiration = field.Value.Content;
                            break;
                        case "Nationality":
                            entity.Nationality = field.Value.Content;
                            break;
                        case "DocumentType":
                            entity.DocumentType = field.Value.Content;
                            break;
                        case "IssuingAuthority":
                            entity.IssuingAuthority = field.Value.Content;
                            break;
                        case "MachineReadableZone":
                            entity.MachineReadableZone = field.Value.Content;
                            break;
                        case "PlaceOfBirth":
                            entity.PlaceOfBirth = field.Value.Content;
                            break;
                        case "Sex":
                            entity.Sex = field.Value.Content;
                            break;
                        case "DocumentNumber":
                            entity.PartitionKey = field.Value.Content;
                            entity.DocumentNumber = field.Value.Content;
                            break;
                    }
                }
            }
        }
    }
}
