using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace AzureBlobService.Common.Services.Storage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string accessKey = string.Empty;
        private readonly string containerName = string.Empty;

        public BlobStorageService()
        {
            accessKey = ConfigurationManager.AppSettings["StorageAccount"];
            containerName = ConfigurationManager.AppSettings["StorageContainer"];
        }

        /// <summary>
        /// Get all the files in the blob.
        /// </summary>
        /// <returns>A collection of urls.</returns>
        public List<Uri> GetAllFilesInBlob()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            cloudBlobContainer.CreateIfNotExists();

            // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  
            // access to the resource. The second approach is to set permissions to allow public access to blobs in this container. 
            // Comment the line below to not use this approach and to use SAS. Then you can view the image using: 
            // https://[InsertYourStorageAccountNameHere].blob.core.windows.net/[InsertYourContainerNameHere]/FileName 
            cloudBlobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Gets all Cloud Block Blobs in the blob container.
            var allBlobs = new List<Uri>();
            foreach (IListBlobItem blob in cloudBlobContainer.ListBlobs())
            {
                if (blob.GetType() == typeof(CloudBlockBlob))
                {
                    allBlobs.Add(blob.Uri);
                }
            }

            return allBlobs;
        }

        /// <summary>
        /// Upload a file to blob storage.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="fileData">The file data.</param>
        /// <param name="fileMimeType">The file mime type.</param>
        /// <returns>The file url.</returns>
        public string UploadFileToBlob(string fileName, byte[] fileData, string fileMimeType)
        {
            string fileUrl = UploadFileToBlobStorage(fileName, fileData, fileMimeType);
            return fileUrl;
        }

        /// <summary>
        /// Delete blob data.
        /// </summary>
        /// <param name="fileUrl">The file url.</param>
        public void DeleteBlobData(string fileUrl)
        {
            var uriObj = new Uri(fileUrl);
            string blobName = Path.GetFileName(uriObj.LocalPath);

            var cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

            string pathPrefix = "/";
            var blobDirectory = cloudBlobContainer.GetDirectoryReference(pathPrefix);

            // Get block blob reference.
            var blockBlob = blobDirectory.GetBlockBlobReference(blobName);

            // Delete blob from container.
            blockBlob.Delete();
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
            var cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            string newFileName = GenerateFileName(fileName);

            if (cloudBlobContainer.CreateIfNotExists())
            {
                cloudBlobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            if (newFileName != null && fileData != null)
            {
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(newFileName);
                cloudBlockBlob.Properties.ContentType = fileMimeType;
                cloudBlockBlob.UploadFromByteArray(fileData, 0, fileData.Length);
                return cloudBlockBlob.Uri.AbsoluteUri;
            }

            return string.Empty;
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