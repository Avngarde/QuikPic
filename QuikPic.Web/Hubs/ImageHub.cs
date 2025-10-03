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

            var editedImage = processor.GetImageRgba32();
            ImageHandler.SaveImageToPath(editedImage, path);

            await Clients.Caller.SendAsync("ImageUpdated", fileNameTrimmed);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("ImageError", ex.Message);
        }
    }
}
