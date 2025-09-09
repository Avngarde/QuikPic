using System;
using System.Security.Cryptography.X509Certificates;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace QuikPic;

public class ImageProcessor
{
    private Image<Rgba32> _image;

    public ImageProcessor(Image<Rgba32> image)
    {
        _image = image;
    }

    public void ProcessImage(EditData editData)
    {
        _image.Mutate(img =>
        {
            img.Brightness(1f + editData.Brightness);
            img.Contrast(1f + editData.Contrast);
            img.Saturate(1f + editData.Saturation);
            img.Grayscale(1f + editData.Grayscale);
        });
    }

    public byte[] GetImageBytes()
    {
        using (var ms = new MemoryStream())
        {
            _image.Save(ms, _image.Metadata.DecodedImageFormat);
            return ms.ToArray();
        }
    }

    public Image<Rgba32> GetImageRgba32()
    {
        return _image;
    }
}
