using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuikPic.Web.Models;

namespace QuikPic.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult UploadFile(IFormFile file)
    {
        string filePath = "";
        string fileGuid = "";
        if (file != null && file.Length > 0) 
        {
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var uniqueFileGuid = Guid.NewGuid().ToString();
            fileGuid = uniqueFileGuid + Path.GetExtension(file.FileName);

            filePath = Path.Combine(uploadDir, uniqueFileGuid + Path.GetExtension(file.FileName));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }

        return RedirectToAction("Index", "Edit", new { fileGuid = fileGuid });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
