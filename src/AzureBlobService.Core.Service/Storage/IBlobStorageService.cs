﻿using System.Collections.Generic;

namespace AzureBlobService.Core.Service.Storage
{
    /// <summary>
    /// The interface for the blob storage service.
    /// </summary>
    public interface IBlobStorageService
    {
        /// <summary>
        /// Get all the files in the blob.
        /// </summary>
        /// <returns>A collection of urls.</returns>
        List<string> GetAllFilesInBlob();

        /// <summary>
        /// Upload a file to blob storage.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="fileData">The file data.</param>
        /// <param name="fileMimeType">The file mime type.</param>
        /// <returns>The file url.</returns>
        string UploadFileToBlob(string fileName, byte[] fileData, string fileMimeType);

        /// <summary>
        /// Delete blob data.
        /// </summary>
        /// <param name="fileUrl">The file url.</param>
        void DeleteBlobData(string fileUrl);
    }
}
