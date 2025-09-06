using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace QuikPic
{
    public static class ImageReader
    {
        public static Image<Rgba32> LoadImageFromPath(string path)
        {
            Image<Rgba32> image = Image.Load<Rgba32>(path);
            return image;
        }

        public static Image<Rgba32> LoadImageFromBytes(byte[] imageData)
        {
            Stream fileStream = new MemoryStream(imageData);
            Image<Rgba32> image = Image.Load<Rgba32>(fileStream);
            fileStream.Close();

            return image;
        }
    }
}
