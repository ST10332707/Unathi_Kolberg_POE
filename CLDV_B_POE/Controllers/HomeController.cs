using CLDV_B_POE.Models;
using CLDV_B_POE.Properties.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;


namespace CLDV_B_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlobService _blobService;
        private readonly TableService _tableService;
        private readonly QueueService _queueService;
        private readonly FileService _fileService;

        public HomeController(BlobService blobService, TableService tableService, QueueService queueService, FileService fileService)
        {
            _blobService = blobService;
            _tableService = tableService;
            _queueService = queueService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                await _blobService.UploadBlobAsync("product-images", file.FileName, stream);
            }
            return RedirectToAction("Privacy");
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
        {
            if (ModelState.IsValid)
            {
                await _tableService.AddEntityAsync(profile);
            }
            return RedirectToAction("Privacy");
        }

        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string orderId)
        {
            await _queueService.SendMessageAsync("order-processing", $"Processing order {orderId}");
            return RedirectToAction("Privacy");
        }

        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                await _fileService.UploadFileAsync("contracts-logs", file.FileName, stream);
            }
            return RedirectToAction("Privacy");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
