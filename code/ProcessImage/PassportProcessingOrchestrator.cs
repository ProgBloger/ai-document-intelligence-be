using Azure.Data.Tables;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace ProcessImage
{
    public static class PassportProcessingOrchestrator
    {
        [Function(nameof(PassportProcessingOrchestrator))]
        public static async Task<OcrResultEntity> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(PassportProcessingOrchestrator));
            logger.LogInformation("Processing passport image.");

            var blobUri = context.GetInput<string>();
            logger.LogInformation($"Received blob URI in orchestrator: {blobUri}");
            var passportOcrEntity = await context.CallActivityAsync<OcrResultEntity>("ProcessWithFormRecognizer", blobUri);

            await context.CallActivityAsync("StoreInTableStorage", passportOcrEntity);

            return passportOcrEntity;
        }

        [Function("PassportProcessingOrchestrator_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("PassportProcessingOrchestrator_HttpStart");

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(PassportProcessingOrchestrator));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
