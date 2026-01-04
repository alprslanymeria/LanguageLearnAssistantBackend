using App.Application.Contracts.Infrastructure.Files;
using App.Application.Contracts.Services;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace App.Integration;

/// <summary>
/// SERVICE FOR IMAGE PROCESSING OPERATIONS.
/// </summary>
public class ImageProcessingService(

    ILogger<ImageProcessingService> logger

    ) : IImageProcessingService
{
    private const string DefaultColor = "#CCCCCC";

    public async Task<string> ExtractLeftSideColorAsync(IFileUpload imageFile)
    {
        try
        {
            using var stream = imageFile.OpenReadStream();
            using var image = await Image.LoadAsync<Rgba32>(stream);

            // RESIZE IMAGE FOR PERFORMANCE
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(100, 100),
                Mode = ResizeMode.Max
            }));

            // GET LEFT SIDE PIXELS (FIRST 10 COLUMNS)
            var leftPixels = new List<Rgba32>();
            for (var x = 0; x < Math.Min(10, image.Width); x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    leftPixels.Add(image[x, y]);
                }
            }

            // CALCULATE AVERAGE COLOR
            var avgR = (int)leftPixels.Average(p => p.R);
            var avgG = (int)leftPixels.Average(p => p.G);
            var avgB = (int)leftPixels.Average(p => p.B);

            var hexColor = $"#{avgR:X2}{avgG:X2}{avgB:X2}";

            logger.LogInformation("ImageProcessingService -> EXTRACTED COLOR: {Color}", hexColor);

            return hexColor;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "ImageProcessingService -> FAILED TO EXTRACT COLOR, USING DEFAULT");
            return DefaultColor;
        }
    }
}
