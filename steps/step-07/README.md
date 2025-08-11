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

---

### Adding Azure Storage Connection String to the Azure Function Trigger

To allow the Azure Function to react to new blob uploads, it needs the **connection string** from Key Vault set in its **environment variables**.

---

**9.** Go to your **Azure Key Vault**, navigate to **Secrets**, and click on the secret named `AzureStorageConnectionString`.
Then, click on the **current version** of the secret.

![key vault](sshot-7-12.png)
![storage con string secret](sshot-7-13.png)

---

**10.** Copy the **Secret Identifier** value — this is the full URL pointing to the secret.

![secret identifier](sshot-7-14.png)

---

**11.** In the Azure Portal, go to your **Function App**, then navigate to:
**Settings > Environment variables**, and click **Add**.

![navigating to function app](sshot-7-15.png)
![func app env variables](sshot-7-16.png)

---

**12.** Add a new environment variable:

* **Name:**

  ```
  AzureStorageConnectionString
  ```
* **Value:**
  Paste the copied URL in this format:

  ```
  @Microsoft.KeyVault(SecretUri=<Your_Secret_Identifier_URL>)
  ```

Example:

```
@Microsoft.KeyVault(SecretUri=https://kvaidocintelligencegwc.vault.azure.net/secrets/AzureStorageConnectionString/12345678912345678962321b4195edac)
```

---

**13.** Click **Apply** in the variable editor, then again on the **Environment Variables** page to save changes.

---

### Adding the Managed Identity to Azure Function and Assigning Key Vault Access

To allow the Azure Function to securely read secrets from Key Vault, you need to enable its **Managed Identity** and assign it the **Key Vault Secrets User** role.

---

**14.** In your **Function App**, go to the **Identity** section in the left menu.
Under the **System assigned** tab, switch the status to **On**, then click **Save** and confirm.

![enabling managed identity](sshot-7-17.png)

---

**15.** Once the managed identity is enabled, wait for it to be created.
Then, click the **Azure role assignments** button that appears.

![managed identity role button](sshot-7-18.png)

---

**16.** On the **Azure role assignments** page, click **Add role assignment**.
Select the **Key Vault** as the scope, choose the **Key Vault Secrets User** role, and click **Save**.

![selecting secrets user role for key vault](sshot-7-19.png)

---

✅ The Azure Function is now fully set up to read secrets from Key Vault and trigger on new files uploaded to Blob Storage.

[Previous step](../step-06/README.md) - [Next step](../step-08/README.md)