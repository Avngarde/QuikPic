using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

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

            var image = ImageReader.LoadImageFromPath(path);
            ImageProcessor imageProcessor = new(image);

            imageProcessor.ProcessImage(editData);
            var imageBytes = imageProcessor.GetImageBytes();

            return File(imageBytes, MediaTypeNames.Image.Jpeg, "image.jpg");
        }
    }
}
