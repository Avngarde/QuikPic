using Microsoft.AspNetCore.Mvc;

namespace QuikPic.Web.Controllers
{
    public class EditController : Controller
    {
        public IActionResult Index(string fileGuid)
        {
            ViewData["fileGuid"] = fileGuid;
            
            return View();
        }

        public IActionResult EditPhoto(EditData editData)
        {
            return View();
        }
    }
}
