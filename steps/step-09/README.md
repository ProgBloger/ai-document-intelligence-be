# AI Document Intelligence

[Previous step](../step-08/README.md) - [Next step](../step-10/README.md)

## Step 9 - Modify the .NET Core Worker Service to call the WebApi and fetch the machine name

1. Open the WorkerService project in Visual Studio Code and use `dotnet add package RestSharp` to add the RestSharp NuGet package:

![installing restsharp package](sshot-9-1.png)

2. Modify the Worker class to fetch the machine name from the WebApi. The Kubernetes service allows the WebApi to be accessed within the cluster using its app-name as the domain name:

```csharp
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using(RestClient _client = new RestClient("http://webapi:8080"))
            {
                var request = new RestRequest("status", Method.Get);
                var response = await _client.ExecuteAsync(request);
                _logger.LogInformation($"RESPONSE: {response.StatusCode}, {response.Content}");
            }
            
            await Task.Delay(500, stoppingToken);
        }
    }
}
```

3. Right-click the Dockerfile in the WorkerService project to build it:

![building the workerservice image](sshot-9-2.png)

4. After a successful build, find the image in your local Docker images and push it to your Azure Container Registry:

![pushing the image](sshot-9-3.png)

5. In Visual Studio Code's Kubernetes Activity pane, delete the WorkerService pod. Kubernetes will automatically spin up a new instance from the Azure Container Registry, due to the deployment's configuration to maintain one replica:

![deleteing workerservice](sshot-9-4.png)

6. Monitor the WorkerService logs and observe that the machine names returned from WebApi vary. This variation occurs because the Kubernetes service for WebApi load balances requests across multiple pods, as dictated by the replica parameter:

![deleteing workerservice](sshot-9-5.png)

[Previous step](../step-08/README.md) - [Next step](../step-10/README.md)