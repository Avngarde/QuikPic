using System;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using QuikPic.Web.Models;

namespace QuikPic.Web.Hubs;

public class ImageHub : Hub
{
    private readonly IWebHostEnvironment _env;
    private readonly QuikPicContext _qpDbContext;

    public ImageHub(IWebHostEnvironment env, QuikPicContext quikPicContext) 
    {
        _env = env;
        _qpDbContext = quikPicContext;
    }

    public async Task ApplyFiltersToImage(EditData editData, string fileName)
    {
        try
        {
            string fileNameTrimmed = fileName.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var path = Path.Combine(_env.WebRootPath, fileNameTrimmed); 
            var image = ImageHandler.LoadImageFromPath(path);

            ImageProcessor processor = new(image);
            processor.ProcessImage(editData);

            string previewFileName = fileNameTrimmed.Replace(".", "_temp.");
            string previewFilePath = Path.Combine(_env.WebRootPath, previewFileName); 
            var editedImage = processor.GetImageRgba32();
            ImageHandler.SaveImageToPath(editedImage, previewFilePath);

            await Clients.Caller.SendAsync("ImageUpdated", previewFileName);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("ImageError", ex.Message);
        }
    }

    public async Task ApplyPresetById(int presetId, string fileName)
    {
        try
        {
            Preset? preset = _qpDbContext.Presets.Where(p => p.Id == presetId).FirstOrDefault();
            if (preset == null) throw new Exception();

            EditData editData = new()
            {
                Brightness = preset.Brightness,
                Contrast = preset.Contrast,
                Saturation = preset.Saturation,
                Grayscale = preset.Grayscale,
                Temperature = preset.Temperature,
                Tint = preset.Tint,
                Grain = preset.Grain,
                Vignette = preset.Vignette
            };

            string fileNameTrimmed = fileName.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var path = Path.Combine(_env.WebRootPath, fileNameTrimmed); 
            var image = ImageHandler.LoadImageFromPath(path);

            ImageProcessor processor = new(image);
            processor.ProcessImage(editData);

            string previewFileName = fileNameTrimmed.Replace(".", "_temp.");
            string previewFilePath = Path.Combine(_env.WebRootPath, previewFileName); 
            var editedImage = processor.GetImageRgba32();
            ImageHandler.SaveImageToPath(editedImage, previewFilePath);

            await Clients.Caller.SendAsync("ImageUpdated", previewFileName);
            await Clients.Caller.SendAsync("PresetUpdateLabels", editData);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("ImageError", ex.Message);
        }
    }
}
