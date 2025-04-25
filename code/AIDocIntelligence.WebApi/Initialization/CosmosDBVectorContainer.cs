using Microsoft.Azure.Cosmos;
using System.Collections.ObjectModel;

namespace AIDocIntelligence.WebApi.Initialization
{
    public static class CosmosDBVectorContainer
    {
        private const int AutoscaleMaxThroughput = 1000;

        public static void CreateVectorContainerIfNotExistsAsync(this Database database, string containerName)
        {
            var throughputProperties = ThroughputProperties.CreateAutoscaleThroughput(AutoscaleMaxThroughput);

            var properties = new ContainerProperties(id: containerName, partitionKeyPath: "/id")
            {
                DefaultTimeToLive = 86400,
                VectorEmbeddingPolicy = new(
                    new Collection<Embedding>(
                        [
                            new Embedding()
                            {
                                Path = "/vectors",
                                DataType = VectorDataType.Float32,
                                DistanceFunction = DistanceFunction.Cosine,
                                Dimensions = 1536
                            }
                        ])),
                IndexingPolicy = new IndexingPolicy()
                {
                    VectorIndexes = new()
                    {
                        new VectorIndexPath()
                        {
                            Path = "/vectors",
                            Type = VectorIndexType.QuantizedFlat,
                        }
                    }
                }
            };

            var container = database.CreateContainerIfNotExistsAsync(properties, throughputProperties).Result;
        }
    }
}
