using System;
using System.Collections.Generic;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureBlobService.Core.Service.Storage
{
    /// <summary>
    /// The blob storage service.
    /// </summary>
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string accessKey = string.Empty;
        private readonly string containerName = string.Empty;
        private readonly ILogger<BlobStorageService> _logger;

        public BlobStorageService(
            IConfiguration config,
            ILogger<BlobStorageService> logger)
        {
            accessKey = config.GetSection("Azure")["StorageAccount"];
            containerName = config.GetSection("Azure")["StorageContainer"];
            _logger = logger;
        }

        public List<string> GetAllFilesInBlob()
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(accessKey);
                var cloudBlobContainer = blobServiceClient.GetBlobContainerClient(containerName);

                // Create the blob if it doesn't exist & Set the blob permissions.
                // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  
                // access to the resource. The second approach is to set permissions to allow public access to blobs in this container. 
                // Comment the line below to not use this approach and to use SAS. Then you can view the image using: 
                // https://[InsertYourStorageAccountNameHere].blob.core.windows.net/[InsertYourContainerNameHere]/FileName 
                cloudBlobContainer.CreateIfNotExists(PublicAccessType.Blob);

                // Create a blob client to use later.
                BlobClient blobClient = null;

                // Gets all Cloud Block Blobs in the blob container.
                var allBlobs = new List<string>();
                foreach (var blob in cloudBlobContainer.GetBlobs())
                {
                    // Get a BlobClient by using blob name.
                    blobClient = cloudBlobContainer.GetBlobClient(blob.Name);
                    string blobUrl = blobClient.Uri.ToString();
                    allBlobs.Add(blobUrl);
                }

                return allBlobs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} : {nameof(GetAllFilesInBlob)} failed to get files - {ex.Message}", ex);
                return new List<string>();
            }
        }

        public string UploadFileToBlob(string fileName, byte[] fileData, string fileMimeType)
        {
            string fileUrl = UploadFileToBlobStorage(fileName, fileData, fileMimeType);
            return fileUrl;
        }

        public void DeleteBlobData(string fileUrl)
        {
            try
            {
                var uriObj = new Uri(fileUrl);
                string blobName = Path.GetFileName(uriObj.LocalPath);

                var blobServiceClient = new BlobServiceClient(accessKey);
                var cloudBlobContainer = blobServiceClient.GetBlobContainerClient(containerName);

                // Get block blob reference.
                var blockBlob = cloudBlobContainer.GetBlobClient(blobName);

                // Delete blob from container.
                blockBlob.Delete();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} : {nameof(DeleteBlobData)} failed to delete data - {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Upload a file to blob storage.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="fileData">THe file data.</param>
        /// <param name="fileMimeType">The file mime type.</param>
        /// <returns>The file url.</returns>
        private string UploadFileToBlobStorage(string fileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(accessKey);
                var cloudBlobContainer = blobServiceClient.GetBlobContainerClient(containerName);
                string newFileName = GenerateFileName(fileName);
                cloudBlobContainer.CreateIfNotExists(PublicAccessType.Blob);

                if (newFileName != null && fileData != null)
                {
                    var cloudBlockBlob = cloudBlobContainer.GetBlobClient(newFileName);
                    cloudBlockBlob.Upload(
                        new MemoryStream(fileData),
                        new BlobHttpHeaders()
                        {
                            ContentType = fileMimeType
                        });
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} : {nameof(UploadFileToBlobStorage)} failed to upload - {ex.Message}", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Generate a file name that appends the date.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The generated file name.</returns>
        private string GenerateFileName(string fileName)
        {
            fileName = fileName.Replace(" ", "_");
            var newFileName = DateTime.UtcNow.ToString("yyyyMMdd\\THHmmssfff") + "." + fileName;
            return newFileName;
        }
    }
}
