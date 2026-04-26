using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using QuikPic.Web.Models;

namespace QuikPic.Web.Controllers
{
    public class EditController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly QuikPicContext _qpDbContext;

        public EditController(IWebHostEnvironment env, QuikPicContext quikPicContext)
        {
            _env = env;
            _qpDbContext = quikPicContext;
        }

        [Route("[controller]/[action]/{fileGuid}/{presetId?}")]
        public IActionResult Index(string fileGuid, int? presetId)
        {
            var presets = _qpDbContext.Presets.ToList();
            Preset? selectedPreset;
            if (presetId is not null)
            {
                selectedPreset = presets.FirstOrDefault(p => p.Id == presetId);
                if (selectedPreset is not null) 
                    ViewData["selectedPreset"] = selectedPreset;
            }

            ViewData["fileGuid"] = fileGuid;
            ViewData["presets"] = presets;

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
