using SixLabors.ImageSharp.PixelFormats;

namespace QuikPic.Tests
{
    public class ImageReaderTests
    {
        [Fact]
        public void LoadImageFromPath_ForTestImagePath_ReturnsRgba32()
        {
            string path = @"./TestImages/TestImage.png";
            var file = ImageReader.LoadImageFromPath(path);

            // TestImage.png is 272 x 193 pixels
            Assert.Equal(272, file.Size.Width);
            Assert.Equal(193, file.Size.Height);
        }

        [Fact]
        public void LoadImageFromBytes_ForTestImagePath_ReturnsRgba32()
        {
            string path = @"./TestImages/TestImage.png";
            byte[] fileBytes = File.ReadAllBytes(path);
            var file = ImageReader.LoadImageFromBytes(fileBytes);
            
            // TestImage.png is 272 x 193 pixels
            Assert.Equal(272, file.Size.Width);
            Assert.Equal(193, file.Size.Height);
        }
    }
}