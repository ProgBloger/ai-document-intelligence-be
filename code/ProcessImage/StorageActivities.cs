using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.DurableTask;
using Azure.Data.Tables;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Microsoft.Extensions.Logging;

using ProcessImage.Common;

namespace ProcessImage
{
    public class StorageActivities
    {
        private readonly SecretProvider _secretProvider;

        public StorageActivities(SecretProvider secretProvider)
        {
            _secretProvider = secretProvider;
        }

        [Function("StoreInTableStorage")]
        public async Task Run([ActivityTrigger] OcrResultEntity entity, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("StoreInTableStorage");
            logger.LogInformation("Storing OCR results in Azure Table Storage.");

            try
            {
                string storageConnStr = await _secretProvider.GetSecretAsync("AzureStorageConnectionString");

                var serviceClient = new TableServiceClient(storageConnStr);
                var tableClient = serviceClient.GetTableClient(ResourceNames.OcrResultsTableName);

                await tableClient.CreateIfNotExistsAsync();
                await tableClient.AddEntityAsync(entity);

                logger.LogInformation("OCR result saved to Table Storage.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception while saving to Table Storage: {ex.Message}");
                logger.LogError(ex.ToString());
            }
        }
    }
}