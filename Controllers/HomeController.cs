using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using OneConnector.Models;
using OneConnector.Services.DataAccess;
using OneConnector.Services.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace OneConnector.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ILogger<HomeController> Logger { get; }
        private ApiTokenAccess ApiTokenAccess { get; }
        private RecordAccess RecordAccess { get; }

        public HomeController(ILogger<HomeController> logger, 
            ApiTokenAccess apiTokenAccess, 
            RecordAccess recordAccess
        )
        {
            Logger = logger;
            ApiTokenAccess = apiTokenAccess;
            RecordAccess = recordAccess;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //string ser = TestCustomerCases.GetTestSerialize();
            //ItemProperties ip = RecordAccess.GetCase("22011086");
            //CaseItems caseItems = RecordAccess.GetAllCaseSubItems("22011086");
            //CustomerCases customerCases = RecordAccess.GetCustomerCases("7001693759");
            //ItemInfo itemInfo = RecordAccess.GetItemInfo("_SkQoQoikqPcP7E7IVFw", "Document");
            return View();
        }

        [HttpGet]
        public IActionResult CustomerCases(string idNumber = "7001693759")
        {
            CustomerCasesModel model = new()
            {
                IdNumber = idNumber,
                CustomerCases = RecordAccess.GetCustomerCases(idNumber)
            };
            return View(model);
        }
        public class CustomerCasesModel
        {
            public CustomerCases CustomerCases { get; set; }
            public string IdNumber { get; set; }
        }

        public IActionResult Case(string caseNumber)
        {
            CaseModel model = new()
            {
                CaseProperties = RecordAccess.GetCase(caseNumber),
                SubItems = RecordAccess.GetAllCaseSubItems(caseNumber)
            };
            return View(model);
        }

        public class CaseModel
        {
            public ItemProperties CaseProperties { get; set; }
            public CaseItems SubItems { get; set; }
        }


        [HttpGet]
        public IActionResult AddDocument(string caseNumber)
        {
            ViewBag.CaseNumber = caseNumber;
            return View();
        }

        [HttpPost]
        public IActionResult AddDocument(
            string caseNumber,
            string documentNumber,
            List<IFormFile> files
        )
        {
            foreach (IFormFile file in files)
            {
                MemoryStream memoryStream = new();
                using (Stream fileStream = file.OpenReadStream())
                {
                    fileStream.CopyTo(memoryStream);
                }
                byte[] bytes = memoryStream.ToArray();
                RecordAccess.AddDocument(caseNumber, documentNumber, file.FileName, User.Identity.Name, bytes);
            }
            return View();
        }

        [HttpGet]
        public IActionResult GetDocument(
            string documentId,
            string documentClass = "Document",
            string fileName = null
        )
        {
            if (fileName == null)
            {
                ItemInfo info = RecordAccess.GetItemInfo(documentId, documentClass);
                fileName = info.Data.Item.Subject.Value;
            }

            string contentType;
            if (!(new FileExtensionContentTypeProvider()).TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return new FileContentResult(RecordAccess.GetDocumentBytes(documentId), contentType)
            {
                FileDownloadName = fileName
            };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
