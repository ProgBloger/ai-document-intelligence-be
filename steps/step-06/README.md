# AI Document Intelligence

[Previous step](../step-05/README.md) - [Next step](../step-07/README.md)

## Step 6 - Deploy the Worker Service inside the Kubernetes cluster

### Pushing WorkerService image into the Azure Container registry

1. Open your WorkerService project in Visual Studio Code, build the Docker image, and locate it in the Docker activity containers pane. Right-click the latest tag and push the image to the container repository:

![pushing image to the repository](sshot-6-1.png)

2. If not logged into Docker registries in Azure, manually connect by clicking the "Connect Registry" button in the error notification and follow the subsequent steps:

![pushing image to the repository](sshot-6-2.png)

![pushing image to the repository](sshot-6-3.png)

![pushing image to the repository](sshot-6-4.png)

3. The Visual Studio Code command palette will ask you to specify the container registry and, if connected to Azure properly, you should be able to find your container registry:

![pushing image to the repository](sshot-6-5.png)

![pushing image to the repository](sshot-6-6.png)

![pushing image to the repository](sshot-6-7.png)

![pushing image to the repository](sshot-6-8.png)

4. If the push was successful, find a copy of your Docker image in the Registries pane:

![pushing image to the repository](sshot-6-9.png)

### Creating namespace in Kubernetes

1. Select Kubernetes in the Visual Studio Code activity pane and open your cluster's namespaces list:

![pushing image to the repository](sshot-6-10.png)

2. Create a new namespace by double-clicking the default namespace and modifying the YAML script:

```yaml
apiVersion: v1
kind: Namespace
metadata:
  name: cloudnativeapp
spec:
  finalizers:
    - kubernetes
status:
  phase: Active
```
3. Open the Visual Studio Code command palette and find the "Kubernetes: Apply" command:

![pushing image to the repository](sshot-6-11.png)

4. Confirm the namespace creation and select it as the active namespace:

![pushing image to the repository](sshot-6-12.png)

![pushing image to the repository](sshot-6-13.png)

### Running Kubernetes pod with WorkerService

1. Create a file named deployment-workerservice.yaml in Visual Studio Code and add the specified content:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: workerservice
spec:
  replicas: 1
  selector:
    matchLabels:
      app: workerservice
  template:
    metadata:
      labels:
        app: workerservice
    spec:
      containers:
      - name: workerservice
        image: acrcloudnativeappwe.azurecr.io/workerservice:latest
        resources:
          limits:
            memory: "128Mi"
            cpu: "500m"
```

2. Use the "Kubernetes: Apply" command to deploy the script and create and run the Kubernetes Pod.

3. Find the running Kubernetes Pod in the Kubernetes activity pane by expanding Clusters, Workloads, Deployments, and Pods:

![pushing image to the repository](sshot-6-14.png)

4. Right-click the running Pod, choose "Logs," and click "Run" to view the console output from the WorkerService:

![pushing image to the repository](sshot-6-15.png)

![pushing image to the repository](sshot-6-16.png)

![pushing image to the repository](sshot-6-17.png)

[Previous step](../step-05/README.md) - [Next step](../step-07/README.md)