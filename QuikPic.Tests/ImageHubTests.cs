using System;
using Xunit;
using QuikPic.Web.Hubs;
using Moq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Hosting;

namespace QuikPic.Tests;

public class ImageHubTests
{
    [Fact]
    public async Task ApplyFiltersToImage_ThrowsNoException()
    {
        var mockEnv = new Mock<IWebHostEnvironment>();
        var mockClients = new Mock<IHubCallerClients>();
        var mockCaller = new Mock<ISingleClientProxy>();

        mockEnv.SetupGet(e => e.ContentRootPath).Returns(Path.GetTempPath());
        mockCaller
            .Setup(c => c.SendCoreAsync(
                "ImageUpdated",
                It.IsAny<object[]>(),
                default))
            .Returns(Task.CompletedTask)
            .Verifiable();

        mockClients.Setup(c => c.Caller).Returns(mockCaller.Object);

        var hub = new ImageHub(mockEnv.Object)
        {
            Clients = mockClients.Object
        };

        EditData editData = new()
        {
            Brightness = 1.0f,
            Contrast = 0.5f,
            Saturation = 0.2f,
            Grayscale = 0.0f
        };

        await hub.ApplyFiltersToImage(editData, "/TestImages/TestImage.png");

        mockCaller.Verify(c => c.SendCoreAsync(
            "ImageUpdated",
            It.Is<object[]>(o => o.Length == 1 && o[0].ToString().EndsWith("TestImage.png")),
            default), Times.Once);
    }
}
