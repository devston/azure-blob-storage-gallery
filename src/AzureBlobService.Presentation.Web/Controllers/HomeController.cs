using AzureBlobService.Presentation.Service.Storage;
using AzureBlobService.Presentation.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace AzureBlobService.Presentation.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlobStoragePresentationService _storageService;

        public HomeController(
            ILogger<HomeController> logger,
            IBlobStoragePresentationService storageService)
        {
            _logger = logger;
            _storageService = storageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            var images = _storageService.GetAllFilesInBlob();
            return View(images);
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        public IActionResult DeleteFile(string fileUrl)
        {
            _storageService.DeleteBlobData(fileUrl);
            return RedirectToAction(nameof(Gallery));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload()
        {
            try
            {
                var files = Request.Form.Files;
                int fileCount = files.Count;

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();

                            // Get the file name and mime type.
                            var fileName = Path.GetFileName(file.FileName);
                            var mimeType = file.ContentType;

                            // Upload the image to blob storage.
                            _storageService.UploadFileToBlob(fileName, fileBytes, mimeType);
                        }
                    }
                }

                return RedirectToAction(nameof(Gallery));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} : Failed to upload file - {ex.Message}.", ex);
                return RedirectToAction(nameof(Error));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
