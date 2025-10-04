using System;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;

namespace QuikPic.Web.Hubs;

public class ImageHub : Hub
{
    private readonly IWebHostEnvironment _env;

    public ImageHub(IWebHostEnvironment env) { _env = env; }

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
}
