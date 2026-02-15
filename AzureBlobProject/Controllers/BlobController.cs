using Azure.Storage.Blobs;
using AzureBlobProject.Models;
using AzureBlobProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobProject.Controllers
{
    public class BlobController : Controller
    {
        private readonly IBlobService _blobService;

        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        public async Task<IActionResult> Manage(string containerName)
        {
            var blobsObject = await _blobService.GetAllBlobs(containerName);
            return View(blobsObject);
        }

        public async Task<IActionResult> AddFile(string containerName)
        {
            return View();
        }

        public async Task<IActionResult> UploadFile(string containerName, IFormFile file)
        {
            if(file== null || file.Length == 0)
            {
                return View();
            }

            var filename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var result = await _blobService.CreateBlob(filename, file, containerName, new BlobModel());
            if (result)
            {
                return RedirectToAction("Index", "Container");
            }
            return View();
        }

    }
}
