
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobProject.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobClient;

        public ContainerService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient containerClient = _blobClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient containerClient = _blobClient.GetBlobContainerClient(containerName);
            await containerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainer()
        {
            List<string> containerNames = new();
            await foreach (var container in _blobClient.GetBlobContainersAsync())
            {
                containerNames.Add(container.Name);
            }
            return containerNames;
        }


        public async Task<List<string>> GetAllContainerAndBlobs()
        {
            List<string> containerAndBlobName = new();

            containerAndBlobName.Add("------ Account Name : " + _blobClient.AccountName + "------");
            await foreach (BlobContainerItem blobContainerItem in _blobClient.GetBlobContainersAsync())
            {
                containerAndBlobName.Add("----" + blobContainerItem.Name);
                BlobContainerClient _blobContainer = _blobClient.GetBlobContainerClient(blobContainerItem.Name);

                await foreach(BlobItem blobItem in _blobContainer.GetBlobsAsync())
                {
                    containerAndBlobName.Add("-" + blobItem.Name);
                }
            }

            return containerAndBlobName;
        }
    }
}
