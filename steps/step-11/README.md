# AI Document Intelligence

[Previous step](../step-10/README.md) - [Next step](../step-12/README.md)

## Step 11 - Locate the Resource Group for Kubernetes Network Resources

The web application is currently exposed only within the Kubernetes cluster. To make it accessible externally, additional configurations are required in Kubernetes and Azure.

1. To find the name of the resource group containing all Kubernetes networking components created by Azure, use the following command:

```
az aks show --resource-group <your resource group> --name <your aks cluster> --query nodeResourceGroup -o tsv
```

```
az aks show --resource-group rg-cloud-native-app-west-europe --name aks-cloud-native-app-we --query nodeResourceGroup -o tsv
```

In this instance, the command returns the following resource group name:

```
MC_rg-cloud-native-app-west-europe_aks-cloud-native-app-we_westeurope
```

2. Locate the resource group and its resources in the Azure Portal using the search feature:

![locating the network group](sshot-11-1.png)

[Previous step](../step-10/README.md) - [Next step](../step-12/README.md)