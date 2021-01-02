using AzureBlobService.Common.Services.Storage;
using System;
using System.IO;
using System.Web.Mvc;

namespace AzureBlobService.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlobStorageService _storageService;

        public HomeController(IBlobStorageService storageService)
        {
            _storageService = storageService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            var images = _storageService.GetAllFilesInBlob();
            return View(images);
        }

        public ActionResult UploadFile()
        {
            return View();
        }

        public ActionResult DeleteFile(string fileUrl)
        {
            _storageService.DeleteBlobData(fileUrl);
            return RedirectToAction("Gallery");
        }

        [HttpPost]
        public ActionResult Upload()
        {
            try
            {
                var files = Request.Files;
                int fileCount = files.Count;

                if (fileCount > 0)
                {
                    for (int i = 0; i < fileCount; i++)
                    {
                        // Get the image bytes.
                        var file = files[i];
                        var target = new MemoryStream();
                        file.InputStream.Position = 0;
                        file.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();

                        // Get the file name and mime type.
                        var fileName = Path.GetFileName(file.FileName);
                        var mimeType = file.ContentType;

                        // Upload the image to blob storage.
                        _storageService.UploadFileToBlob(fileName, data, mimeType);
                    }
                }

                return RedirectToAction("Gallery");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }
    }
}