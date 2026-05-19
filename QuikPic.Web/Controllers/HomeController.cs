using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuikPic.Web.Errors;
using QuikPic.Web.Models;
using SixLabors.ImageSharp;

namespace QuikPic.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _env;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public IActionResult Index(Error? error)
    {
        if (error is null || error.ErrorMessage is not null)
            ViewData["Error"] = error;

        return View();
    }

    [HttpPost]
    public IActionResult UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return RedirectToAction("Index", "Home", new Error { 
                ErrorMessage = "Selected file is not a valid image", ErrorType = ErrorType.FileIsNotAnImage 
            });
        }

        var uploadDir = Path.Combine(_env.WebRootPath, "uploads");

        if (!Directory.Exists(uploadDir))
            Directory.CreateDirectory(uploadDir);

        var fileExtension = Path.GetExtension(file.FileName);
        var fileGuid = Guid.NewGuid().ToString();
        var fileName = fileGuid + fileExtension;
        var filePath = Path.Combine(uploadDir, fileName);

        using (var stream = file.OpenReadStream())
        {
            if (!CheckIfImage(stream))
            {
                return RedirectToAction("Index", "Home", new Error { 
                    ErrorMessage = "Selected file is not a valid image", ErrorType = ErrorType.FileIsNotAnImage 
                });
            }
        }

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        return RedirectToAction("Index", "Edit", new { fileGuid = fileName });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool CheckIfImage(Stream stream)
    {
        try
        {
            using (var img = Image.Load(stream))
            {
                return true;
            }
        }
        catch (UnknownImageFormatException)
        {
            return false;
        }
    }
}
