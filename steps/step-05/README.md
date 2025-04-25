# AI Document Intelligence

[Previous step](../step-04/README.md) - [Next step](../step-06/README.md)

## Step 5 - Create the ACR (Azure Container Registry) resource in your Azure subscription

### Create the container registry

1. Search for "Container Registries" in the Azure portal:

![finding container regestries](sshot-5-1.png)

2. Specify the container registry name and region:

```
acrCloudNativeAppWE
```

![creating container regestries](sshot-5-2.png)

No further changes needed; complete the wizard to create the container registry.

### Link the container registry to Kubernetes

You are going to grant the Kubernetes cluster access to the container registry to push and pull Docker images

1. Run the command from the Visual Studio Code terminal to grant an AKS cluster access to an ACR:

```
az aks update -g <resource-group> -n <aks-name> --attach-acr <acr-name>
```

```
az aks update -g rg-cloud-native-app-west-europe -n aks-cloud-native-app-we --attach-acr acrCloudNativeAppWE
```

2. In Visual Studio Code, open Kubernetes from the Activity Bar and select your cluster in the Azure Cloud pane:

![merge kubernetes cluster into local kubeconfig](sshot-5-3.png)

3. Right-click your Kubernetes cluster and merge it into your local Kubeconfig:

![merge kubernetes cluster into local kubeconfig](sshot-5-4.png)

4. Set your Kubernetes cluster as the current cluster in the Clusters pane:

![set Kubernetes cluster as current](sshot-5-5.png)

[Previous step](../step-04/README.md) - [Next step](../step-06/README.md)