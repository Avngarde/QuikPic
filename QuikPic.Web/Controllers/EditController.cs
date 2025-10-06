using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace QuikPic.Web.Controllers
{
    public class EditController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public EditController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index(string fileGuid)
        {
            ViewData["fileGuid"] = fileGuid;

            return View();
        }

        [HttpPost]
        public IActionResult EditPhoto([FromForm] EditData editData, string fileName)
        {
            string fileNameTrimmed = fileName
                .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .Replace(".", "_temp.");
                
            var path = Path.Combine(_env.WebRootPath, fileNameTrimmed);
            var image = ImageHandler.LoadImageFromPath(path);

            ImageProcessor imageProcessor = new(image);
            var imageBytes = imageProcessor.GetImageBytes();

            string downloadFileName = fileName.Replace("/uploads/", "");

            System.IO.File.Delete(Path.Combine(_env.WebRootPath, fileNameTrimmed));
            return File(imageBytes, MediaTypeNames.Image.Jpeg, downloadFileName);
        }
    }
}
