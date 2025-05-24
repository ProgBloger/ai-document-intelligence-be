# AI Document Intelligence

[Previous step](../step-06/README.md) - [Next step](../step-08/README.md)

## Step 7 - Create and Deploy Azure Function

**1.** Navigate to the `ProcessImage` folder in your terminal and run the command:

```
code .
```

![azure function workspace](sshot-7-1.png)

This will open the Azure Function project in Visual Studio Code.


Click the **Azure Function** icon in the Azure tab in Visual Studio Code, then select **Deploy to Azure** from the dropdown menu.

![azure function icon](sshot-7-2.png)

**2.** When prompted, select the folder containing your Azure Function project — the folder is named `ProcessImage`.

![azure function folder](sshot-7-3.png)

**3.** Choose **Create new Function App...**, and name it:

```
fa-ai-doc-intelligence-gwc
```

![create azure function app](sshot-7-4.png)
![azure function app name](sshot-7-5.png)

**4.** When prompted to select a location, choose **Germany West Central** for this example.

![selecting location](sshot-7-6.png)

**5.** Select the .Net stack

![.net stack selection](sshot-7-7.png)

**6.** Select **Managed Identity** as the authentication method, since the function will access secrets from Azure Key Vault and needs appropriate permissions.

![selecting secrets access](sshot-7-8.png)

**7.** If you see the error like *"The subscription is not registered to use namespace 'Microsoft.Web'"*, fix it by running the following commands in your terminal:

```
az provider register --namespace Microsoft.Web

az provider register --namespace Microsoft.Insights
```

![ms web not registered](sshot-7-9.png)

Then retry the deployment steps

**8.** Go to the **Function App** section in the Azure Portal and verify that `fa-ai-doc-intelligence-gwc` has been successfully deployed and is in a **Running** state.

![function app navigation](sshot-7-10.png)

![verifying function app deployment](sshot-7-11.png)

[Previous step](../step-06/README.md) - [Next step](../step-08/README.md)