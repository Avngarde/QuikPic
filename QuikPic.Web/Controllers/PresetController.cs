using Microsoft.AspNetCore.Mvc;
using QuikPic.Web.Models;
using QuikPic.Web.Services;

namespace QuikPic.Web.Controllers
{
    public class PresetController : Controller
    {
        private readonly IPresetService _presetService;

        public PresetController(IPresetService presetService)
        {
            _presetService = presetService;
        }

        public ActionResult AddPreset([FromForm] Preset preset, string fileName)
        {
            _presetService.Add(preset);

            var fileGuid = fileName.Replace("/uploads/", "");
            int presetId = preset.Id;
            return RedirectToAction("Index", "Edit", new { fileGuid, presetId  });
        }

        public ActionResult EditPreset([FromForm] Preset preset, string fileName)
        {
            _presetService.EditPreset(preset);

            var fileGuid = fileName.Replace("/uploads/", "");
            return RedirectToAction("Index", "Edit", new { fileGuid });
        }

        public ActionResult DeletePreset(int id, string fileName)
        {
            _presetService.DeleteById(id);

            var fileGuid = fileName.Replace("/uploads/", "");
            return RedirectToAction("Index", "Edit", new { fileGuid });
        }
    }
}
