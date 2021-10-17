using AzureBlobService.Core.Service.Storage;
using System.Collections.Generic;

namespace AzureBlobService.Presentation.Service.Storage
{
    /// <summary>
    /// The blob storage presentation service.
    /// </summary>
    public class BlobStoragePresentationService : IBlobStoragePresentationService
    {
        private readonly IBlobStorageService _blobService;

        public BlobStoragePresentationService(
            IBlobStorageService blobService)
        {
            _blobService = blobService;
        }

        public List<string> GetAllFilesInBlob()
        {
            return _blobService.GetAllFilesInBlob();
        }

        public string UploadFileToBlob(string fileName, byte[] fileData, string fileMimeType)
        {
            return _blobService.UploadFileToBlob(fileName, fileData, fileMimeType);
        }

        public void DeleteBlobData(string fileUrl)
        {
            _blobService.DeleteBlobData(fileUrl);
        }
    }
}
