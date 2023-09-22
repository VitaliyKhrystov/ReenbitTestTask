using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;


namespace WebAppReenbitTest.Services
{
    public class FileService
    {
        public readonly string storageAccount;
        public readonly string key;
        public readonly StorageSharedKeyCredential credential;
        public readonly BlobServiceClient blobServiceClient;

        public FileService()
        {
            storageAccount = BlobCredential.Account;
            key = BlobCredential.Key;
            credential = new StorageSharedKeyCredential(storageAccount, key);
            var blobUri = $"https://{storageAccount}.blob.core.windows.net";
            blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        }

        public async Task<int> UploadFileAsync(IFormFile file, string email)
        {

            try
            {
                if (file == null || file.Length == 0 || email == null)
                    return StatusCodes.Status500InternalServerError;

                var containerName = "files";
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                var blobName = $"{email}#{file.FileName}";
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = blobName,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow,
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                };
                sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);

                var sasToken = sasBuilder.ToSasQueryParameters(credential);

                var blobUriWithSas = new BlobUriBuilder(blobClient.Uri) { Sas = sasToken }.ToUri();

                BlobClient blobClientWithSas = new BlobClient(blobUriWithSas);

                using (Stream stream = file.OpenReadStream())
                {
                    var response = await blobClientWithSas.UploadAsync(stream);
                    return response.GetRawResponse().Status;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
