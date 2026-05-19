using Microsoft.AspNetCore.Mvc;
using QuikPic.Web.Errors;
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
            var fileGuid = fileName.Replace("/uploads/", "");
            int presetId = preset.Id;

            try
            {
                if (preset.Name is null) 
                {
                    return RedirectToAction("Index", "Edit", new
                    {
                        fileGuid,
                        presetId,
                        ErrorMessage = "Invalid preset name",
                        ErrorType = ErrorType.AddPresetError
                    });
                }

                _presetService.Add(preset);

                presetId = preset.Id;

                return RedirectToAction("Index", "Edit", new { fileGuid, presetId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Edit", new
                {
                    fileGuid,
                    presetId,
                    ErrorMessage = "Failed to add new preset",
                    ErrorType = ErrorType.AddPresetError
                });
            }
        }

        [HttpPost]
        public ActionResult EditPreset([FromForm] Preset preset, string fileName)
        {
            var fileGuid = fileName.Replace("/uploads/", "");
            var presetId = preset.Id;

            try
            {
                if (preset.Name is null)
                {
                    return RedirectToAction("Index", "Edit", new
                    {
                        fileGuid,
                        presetId,
                        ErrorMessage = "Invalid preset name",
                        ErrorType = ErrorType.EditPresetError
                    });                    
                }

                _presetService.EditPreset(preset);

                return RedirectToAction("Index", "Edit", new { fileGuid, presetId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Edit", new
                {
                    fileGuid,
                    presetId,
                    ErrorMessage = "Failed to edit preset",
                    ErrorType = ErrorType.EditPresetError
                });                
            }
        }

        public ActionResult DeletePreset(int id, string fileName)
        {
            _presetService.DeleteById(id);

            var fileGuid = fileName.Replace("/uploads/", "");
            return RedirectToAction("Index", "Edit", new { fileGuid });
        }
    }
}
