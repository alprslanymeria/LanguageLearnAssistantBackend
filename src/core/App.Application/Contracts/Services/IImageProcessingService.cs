using App.Application.Contracts.Infrastructure.Files;

namespace App.Application.Contracts.Services;

/// <summary>
/// SERVICE FOR IMAGE PROCESSING OPERATIONS.
/// THIS PROVIDES CENTRALIZED IMAGE PROCESSING LOGIC ACROSS HANDLERS.
/// </summary>
public interface IImageProcessingService
{
    /// <summary>
    /// EXTRACTS THE DOMINANT COLOR FROM THE LEFT SIDE OF AN IMAGE.
    /// USED FOR UI COLOR MATCHING WITH BOOK COVERS.
    /// </summary>
    /// <param name="imageFile">THE IMAGE FILE TO PROCESS</param>
    /// <returns>HEX COLOR CODE (E.G., #CCCCCC)</returns>
    Task<string> ExtractLeftSideColorAsync(IFileUpload imageFile);
}
