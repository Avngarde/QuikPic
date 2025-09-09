using System;
using System.Security.Cryptography.X509Certificates;

namespace QuikPic.Tests;


public static class EditDataTestCases
{
    public static IEnumerable<object[]> EditDataAndExpectedResultCases =>
        new List<object[]>
        {
            new object[]
            {
                new EditData { Brightness = -0.20f, Contrast = +0.15f, Grayscale = -0.05f, Saturation = 2f  },
                new EditData { Brightness = 0.8f, Contrast = +1.15f, Grayscale = +0.95f, Saturation = 3f } // expectedResult
            },
            new object[]
            {
                new EditData { Brightness = +0.01f, Contrast = -0.05f, Grayscale = -0.25f, Saturation = -0.3f  },
                new EditData { Brightness = 1.01f, Contrast = +0.95f, Grayscale = +0.75f, Saturation = 0.7f } // expectedResult
            },
        };
        
        public static IEnumerable<object[]> EditDataCases =>
        new List<object[]>
        {
            new object[]
            {
                new EditData { Brightness = -0.20f, Contrast = +0.15f, Grayscale = -0.05f, Saturation = 2f  },
            },
            new object[]
            {
                new EditData { Brightness = +0.01f, Contrast = -0.05f, Grayscale = -0.25f, Saturation = -0.3f  },
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

    [Theory]
    [MemberData(nameof(EditDataTestCases.EditDataAndExpectedResultCases), MemberType = typeof(EditDataTestCases))]
    public void ForFilterData_CalculateFilters_ReturnsValidCalculations(EditData editData, EditData expectedData)
    {
        float grayscale = 1f + editData.Grayscale;
        float contrast = 1f + editData.Contrast;
        float brightness = 1f + editData.Brightness;
        float saturation = 1f + editData.Saturation;

        Assert.Equal(grayscale, expectedData.Grayscale);
        Assert.Equal(contrast, expectedData.Contrast);
        Assert.Equal(brightness, expectedData.Brightness);
        Assert.Equal(saturation, expectedData.Saturation);
    }
}
