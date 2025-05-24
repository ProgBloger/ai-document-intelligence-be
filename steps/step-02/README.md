# AI Document Intelligence

[Previous step](../step-01/README.md) - [Next step](../step-03/README.md)

## Step 2 - Create and Deploy the Azure Key Vault

**1.** In the Azure Portal, search for "Key Vaults" and click Create to start the setup.

![finding azure key vault](sshot-2-1.png)

**2.** Fill in the details using the same resource group you created in the previous step `rg-ai-doc-intelligence-gwc`, and choose the Germany West Central region.

![Creating azure key vault](sshot-2-2.png)

Use a consistent and descriptive name, such as:

```
kvAiDocIntelligenceGwc
```

**3.** After creating the Key Vault, go to it, open Access control (IAM), click Add > Add role assignment, search for Key Vault Administrator, click it, then click Next to proceed with assigning

![navigating to access control](sshot-2-3.png)

![navigation to rola management](sshot-2-4.png)

![navigation to administrator role](sshot-2-5.png)

**4.** On the Members tab click Select members and add your own account by clicking Select

![adding administrator role](sshot-2-6.png)

**5.** Click Review + assign

![adding administrator role](sshot-2-7.png)

Now you can add Secrets to your Azure Key Vault

[Previous step](../step-01/README.md) - [Next step](../step-03/README.md)