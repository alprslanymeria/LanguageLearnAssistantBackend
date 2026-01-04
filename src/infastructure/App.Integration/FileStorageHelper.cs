using App.Application.Contracts.Infrastructure.Files;
using App.Application.Contracts.Infrastructure.Storage;
using App.Application.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace App.Integration;

/// <summary>
/// HELPER SERVICE FOR COMMON FILE STORAGE OPERATIONS.
/// </summary>
public class FileStorageHelper(

    IStorageService storageService,
    ILogger<FileStorageHelper> logger

    ) : IFileStorageHelper
{
    public async Task<string> UploadFileToStorageAsync(IFileUpload file, string userId, string folderName)
    {
        var fileName = $"{userId}/{folderName}/{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}_{file.FileName}";

        using var stream = file.OpenReadStream();
        var fileUrl = await storageService.UploadAsync(stream, fileName, file.ContentType, null);

        logger.LogInformation("FileStorageHelper -> SUCCESSFULLY UPLOADED FILE TO STORAGE: {FileUrl}", fileUrl);

        return fileUrl;
    }

    public async Task DeleteFileFromStorageAsync(string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return;
        }

        try
        {
            var fileExists = await storageService.ExistsAsync(fileUrl);

            if (!fileExists)
            {
                return;
            }

            await storageService.DeleteAsync(fileUrl);
            logger.LogInformation("FileStorageHelper -> SUCCESSFULLY DELETED FILE FROM STORAGE: {FileUrl}", fileUrl);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "FileStorageHelper -> FAILED TO DELETE FILE FROM STORAGE: {FileUrl}", fileUrl);
        }
    }
}
