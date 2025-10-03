using System;
using System.Security.Cryptography.X509Certificates;

namespace QuikPic.Tests;


public static class EditDataTestCases
{ 
    public static IEnumerable<object[]> EditDataCases =>
    new List<object[]>
    {
        new object[]
        {
            new EditData { Brightness = 0.00f, Contrast = 0.15f, Grayscale = 0.05f, Saturation = 2f  },
        },
        new object[]
        {
            new EditData { Brightness = 0.01f, Contrast = 0.05f, Grayscale = 0.25f, Saturation = 0.3f  },
        },           
    };    
}


public class ImageProcessorTests
{
    [Theory]
    [MemberData(nameof(EditDataTestCases.EditDataCases), MemberType = typeof(EditDataTestCases))]
    public void ForImage_ApplyFilters_ChecksForExceptions(EditData editData)
    {
        using var image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(10, 10);

        ImageProcessor processor = new(image);
        processor.ProcessImage(editData);
        var exception = Record.Exception(() => processor.GetImageRgba32());

        Assert.Null(exception);
    }
}
