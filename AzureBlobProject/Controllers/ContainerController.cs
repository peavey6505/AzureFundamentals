using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobProject.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IContainerService _containerService;

        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }

        public async Task<IActionResult> Index()
        {
            var allContainers = await _containerService.GetAllContainer();
            return View(allContainers);
        }

        // GET: /Container/CreateNew
        [HttpGet]
        public IActionResult CreateNew()
        {
            // Returns the Create view (Views/Container/Create.cshtml)
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(string containerName)
        {
            await _containerService.CreateContainer(containerName);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string containerName)
        {
            await _containerService.DeleteContainer(containerName);
            return RedirectToAction("Index");
        }

    }
}
