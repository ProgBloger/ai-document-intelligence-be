# AI Document Intelligence

[Previous step](../step-04/README.md) - [Next step](../step-06/README.md)

## Step 5 - Deploy the Backend API (with Blob Storage & Key Vault integration)

1. Ensure Docker is running on your machine.  

2. Open the Web API project in Visual Studio Code, right-click the **Dockerfile**, and select **Build Image...** from the context menu.

![building the docker image](sshot-5-1.png)

3. When prompted, name the image `aidocintelligence-be:latest` and press **Enter**.

![selecting the image name](sshot-5-2.png)

4. In the Docker extenstion of the visual studio code login into the azure container registry if needed

![login to container registry](sshot-5-3.png)

5. Right-click the backend image, select Push, then follow the prompts to choose your Azure subscription and container registry to push the image.

![pushing image to container registry](sshot-5-4.png)

![selecting container registry](sshot-5-5.png)

6. After pushing the image, go to your container registry in the Azure Portal, search for **Repositories**, and confirm that the image named `aidocintelligence-be` appears in the list.

![navigating to container registry](sshot-5-6.png)

![expected image in container registry](sshot-5-7.png)

[Previous step](../step-04/README.md) - [Next step](../step-06/README.md)