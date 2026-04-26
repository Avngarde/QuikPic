using System;

namespace QuikPic.Web.Models;

public class Preset
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Brightness { get; set; }
    public float Contrast { get; set; }
    public float Saturation { get; set; }
    public float Grayscale { get; set; }
    public float Temperature { get; set; }
    public float Tint { get; set; }
    public float Vignette { get; set; }
}
