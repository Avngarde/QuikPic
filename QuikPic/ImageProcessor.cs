using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace QuikPic
{
    public class ImageProcessor
    {
        private Image<Rgba32> _image;

        public ImageProcessor(Image<Rgba32> image)
        {
            _image = image;
        }

        public void ProcessImage(EditData editData)
        {
            // --- Note: Brightness/Contrast/Saturation expected as multipliers (1.0 = neutral) ---
            if (editData.Brightness >= 0)
                _image.Mutate(x => x.Brightness(editData.Brightness));

            if (Math.Abs(editData.Contrast - 1f) > 0.0001f)
                _image.Mutate(x => x.Contrast(editData.Contrast));

            if (Math.Abs(editData.Saturation - 1f) > 0.0001f)
                _image.Mutate(x => x.Saturate(editData.Saturation));

            if (editData.Grayscale > 0f)
                _image.Mutate(x => x.Grayscale(editData.Grayscale));

            // Apply temperature/tint AFTER the basic adjustments so additive shifts are not multiplied by brightness.
            if (Math.Abs(editData.Temperature) > 0.0001f)
                ApplyTemperature(_image, editData.Temperature);

            if (Math.Abs(editData.Tint) > 0.0001f)
                ApplyTint(_image, editData.Tint);

            if (editData.Vignette > 0f)
                ApplyVignette(_image, editData.Vignette);

            if (editData.Grain > 0f)
                AddGrain(_image, editData.Grain);
        }

        private static void ApplyTemperature(Image<Rgba32> image, float temperature)
        {
            // temperature: -1 (cool) -> +1 (warm)
            // Use a modest additive shift so it won't blow out when brightness changed.
            float adjustment = temperature * 20f; // tweak this factor if you want stronger/weaker effect

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgba32> row = accessor.GetRowSpan(y);
                    for (int x = 0; x < row.Length; x++)
                    {
                        ref Rgba32 p = ref row[x];

                        // Additively warm/cool: raise red, lower blue for warm; inverse for cool.
                        float r = p.R + adjustment;
                        float b = p.B - adjustment;

                        p.R = (byte)Math.Clamp((int)Math.Round(r), 0, 255);
                        p.B = (byte)Math.Clamp((int)Math.Round(b), 0, 255);
                    }
                }
            });
        }

        private static void ApplyTint(Image<Rgba32> image, float tint)
        {
            // tint: -1 (green) -> +1 (magenta)
            float adjustment = tint * 20f;

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgba32> row = accessor.GetRowSpan(y);
                    for (int x = 0; x < row.Length; x++)
                    {
                        ref Rgba32 p = ref row[x];
                        float g = p.G + adjustment;
                        p.G = (byte)Math.Clamp((int)Math.Round(g), 0, 255);
                    }
                }
            });
        }

        private static void ApplyVignette(Image<Rgba32> image, float intensity)
        {
            // intensity range assumed 0..2 (0 none, 1 moderate)
            float strength = Math.Clamp(intensity / 2f, 0f, 1f);
            image.Mutate(ctx => ctx.Vignette(new GraphicsOptions
            {
                AlphaCompositionMode = PixelAlphaCompositionMode.SrcOver,
                BlendPercentage = strength
            }));
        }

        private static void AddGrain(Image<Rgba32> image, float intensity)
        {
            // intensity: 0..3 (tune as you like)
            var random = new Random();
            int grainAmount = (int)(image.Width * image.Height * 0.001f * (intensity)); // density scaling

            image.ProcessPixelRows(accessor =>
            {
                for (int i = 0; i < grainAmount; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    Span<Rgba32> row = accessor.GetRowSpan(y);
                    ref Rgba32 p = ref row[x];

                    int delta = random.Next(-20, 21);
                    p.R = (byte)Math.Clamp(p.R + delta, 0, 255);
                    p.G = (byte)Math.Clamp(p.G + delta, 0, 255);
                    p.B = (byte)Math.Clamp(p.B + delta, 0, 255);
                }
            });
        }

        public byte[] GetImageBytes()
        {
            using var ms = new MemoryStream();
            // Use a specific encoder to avoid relying on Metadata.DecodedImageFormat
            var encoder = new JpegEncoder { Quality = 90 };
            _image.Save(ms, encoder);
            return ms.ToArray();
        }

        public Image<Rgba32> GetImageRgba32()
        {
            return _image;
        }
    }
}