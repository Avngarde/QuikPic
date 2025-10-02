using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace QuikPic
{
    public static class ImageHandler
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

        public static void SaveImageToPath(Image<Rgba32> image, string path)
        {
            image.Save(path);
        }
    }
}
