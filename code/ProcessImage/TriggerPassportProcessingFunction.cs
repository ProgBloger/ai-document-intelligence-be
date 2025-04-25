using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace ProcessImage
{
    public class TriggerPassportProcessingFunction
    {
        private readonly ILogger<TriggerPassportProcessingFunction> _logger;

        public TriggerPassportProcessingFunction(ILogger<TriggerPassportProcessingFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(TriggerPassportProcessingFunction))]
        public async Task Run(
            [BlobTrigger("passports/{name}", Connection = "AIDocIntelligenceStorage")] Stream blobStream,
            string name,
            [DurableClient] DurableTaskClient starter,
            CancellationToken cancellationToken)
        {
            string instanceId = await starter.ScheduleNewOrchestrationInstanceAsync(nameof(PassportProcessingOrchestrator), name);
            _logger.LogInformation($"Started orchestration with ID = {instanceId} for blob {name}.");
        }
    }
}
