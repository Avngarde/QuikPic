using Microsoft.AspNetCore.Mvc;
using QuikPic.Web.Models;

namespace QuikPic.Web.Controllers
{
    public class PresetController : Controller
    {
        private QuikPicContext qpDbContext;

        public PresetController(QuikPicContext quikPicContext)
        {
            qpDbContext = quikPicContext;
        }

        public ActionResult AddPreset([FromForm] Preset preset, string fileName)
        {
            qpDbContext.Add(preset);
            qpDbContext.SaveChanges();

            var fileGuid = fileName.Replace("/uploads/", "");
            return RedirectToAction("Index", "Edit", new { fileGuid });
        }

    }
}
