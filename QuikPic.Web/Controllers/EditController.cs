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

        public IActionResult EditPhoto(EditData editData, string fileName)
        {
            string fileNameTrimmed = fileName.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var path = Path.Combine(_env.WebRootPath, fileNameTrimmed);
            /*
            var image = ImageHandler.LoadImageFromPath(path);
            ImageProcessor imageProcessor = new(image);
            imageProcessor.ProcessImage(editData);
            var imageBytes = imageProcessor.GetImageBytes();
            */
            var imageBytes = System.IO.File.ReadAllBytes(path);
            string downloadFileName = fileName.Replace("/uploads/", "");
            return File(imageBytes, MediaTypeNames.Image.Jpeg, downloadFileName);
        }
    }
}
