using System;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using QuikPic.Web.Models;
using QuikPic.Web.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace QuikPic.Web.Hubs;

public class ImageHub : Hub
{
    private readonly IWebHostEnvironment _env;

    public ImageHub(IWebHostEnvironment env) 
    {
        _env = env;
    }

    private string CreatePreviewFile(Image<Rgba32> image, string fileName)
    {
        string previewFileName = fileName.Replace(".", "_temp.");
        string previewFilePath = Path.Combine(_env.WebRootPath, previewFileName); 
        ImageHandler.SaveImageToPath(image, previewFilePath); 

        return previewFileName;      
    }

    private Image<Rgba32> ApplyFiltersToImage(EditData editData, Image<Rgba32> image)
    {
        ImageProcessor processor = new(image);
        processor.ProcessImage(editData);

        return processor.GetImageRgba32();
    }

    private Image<Rgba32> LoadImageFromFileName(string fileName)
    {
        var path = Path.Combine(_env.WebRootPath, fileName); 
        var image = ImageHandler.LoadImageFromPath(path); 

        return image;      
    }

    private string TrimFileName(string fileName) => 
        fileName.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

    public async Task ApplyFilters(EditData editData, string fileName)
    {
        try
        {
            var fileNameTrimmed = TrimFileName(fileName);
            using var image = LoadImageFromFileName(fileNameTrimmed);
            using var editedImage = ApplyFiltersToImage(editData, image);

            var previewFileName = CreatePreviewFile(editedImage, fileNameTrimmed);
            await Clients.Caller.SendAsync("ImageUpdated", previewFileName);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("ImageError", ex.Message);
        }
    }

    public async Task ApplyPresetById(Preset preset, string fileName)
    {
        try
        {
            EditData editData = new()
            {
                Brightness = preset.Brightness,
                Contrast = preset.Contrast,
                Saturation = preset.Saturation,
                Grayscale = preset.Grayscale,
                Temperature = preset.Temperature,
                Tint = preset.Tint,
                Vignette = preset.Vignette
            };

            var fileNameTrimmed = TrimFileName(fileName);
            using var image = LoadImageFromFileName(fileNameTrimmed);
            using var editedImage = ApplyFiltersToImage(editData, image);

            var previewFileName = CreatePreviewFile(editedImage, fileNameTrimmed);

            await Clients.Caller.SendAsync("ImageUpdated", previewFileName);
            await Clients.Caller.SendAsync("PresetUpdateLabels", editData);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("ImageError", ex.Message);
        }
    }
}
