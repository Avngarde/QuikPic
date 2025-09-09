using System;
using System.Security.Cryptography.X509Certificates;
using SixLabors.ImageSharp;
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
        throw new NotImplementedException();
    }

    public byte GetImageRgba32()
    {
        throw new NotImplementedException();
    }
}
