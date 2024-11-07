using Microsoft.AspNetCore.Mvc;
using Xml.io.controller;

namespace WebXml.Controllers
{
    public class FilesController : Controller
    {
        private readonly DataAccess _dataAccess;
        private readonly XmlParser _xmlParser;
        private readonly HtmlRenderer _htmlRenderer;

        public FilesController()
        {
            string projectRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string databasePath = Path.Combine(projectRootPath, "database.db");
            string xsltPath = Path.Combine(projectRootPath, "template.xslt");

            _dataAccess = new DataAccess(databasePath);
            _xmlParser = new XmlParser();
            _htmlRenderer = new HtmlRenderer(xsltPath);
        }

        // GET: Files
        public IActionResult Index()
        {
            var fileContents = _dataAccess.GetAllFileContents().FirstOrDefault();
            if (fileContents != null)
            {
                var attributes = _xmlParser.ExtractAttributes(fileContents);
                var htmlContent = _htmlRenderer.RenderHtml(attributes);
                ViewBag.TransformedHtml = htmlContent;
            }

            return View();
        }


        // GET: Files/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Files/Upload
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select a file to upload.";
                return View();
            }

            var uniqueFileName =
                $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uniqueFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            byte[] fileContent;

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileContent = memoryStream.ToArray();
                }

                _dataAccess.SaveFileToDatabase(uniqueFileName, fileContent);

                ViewBag.SuccessMessage = "File uploaded successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while uploading the file. Please try again.";
                return View();
            }
        }

        // GET: Files/Display
        public IActionResult Display(int id)
        {
            return View();
        }
    }
}