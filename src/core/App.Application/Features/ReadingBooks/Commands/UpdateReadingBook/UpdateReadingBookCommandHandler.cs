using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Files;
using App.Application.Contracts.Infrastructure.Storage;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingBooks.Dtos;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Net;

namespace App.Application.Features.ReadingBooks.Commands.UpdateReadingBook;

/// <summary>
/// HANDLER FOR UPDATE READING BOOK COMMAND.
/// </summary>
public class UpdateReadingBookCommandHandler(
    IReadingBookRepository readingBookRepository,
    IReadingRepository readingRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService,
    IMapper mapper,
    ILogger<UpdateReadingBookCommandHandler> logger
    ) : ICommandHandler<UpdateReadingBookCommand, ServiceResult<ReadingBookDto>>
{
    public async Task<ServiceResult<ReadingBookDto>> Handle(
        UpdateReadingBookCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateReadingBookCommandHandler -> UPDATING READING BOOK WITH ID: {Id}", request.Id);

        // VERIFY READING BOOK EXISTS
        var existingBook = await readingBookRepository.GetByIdAsync(request.Id);

        if (existingBook is null)
        {
            logger.LogWarning("UpdateReadingBookCommandHandler -> READING BOOK NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<ReadingBookDto>.Fail("READING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        // VERIFY OR CREATE READING
        var readingResult = await VerifyOrCreateReadingAsync(request.ReadingId, request.UserId, request.LanguageId);

        if (!readingResult.IsSuccess)
        {
            return ServiceResult<ReadingBookDto>.Fail(readingResult.ErrorMessage!, readingResult.Status);
        }

        var reading = readingResult.Data!;

        try
        {
            // STORE OLD FILE URLS FOR DELETION
            var oldImageUrl = existingBook.ImageUrl;
            var oldSourceUrl = existingBook.SourceUrl;

            // UPDATE IMAGE IF NEW FILE PROVIDED
            if (request.ImageFile is not null)
            {
                logger.LogInformation("UpdateReadingBookCommandHandler -> UPLOADING NEW IMAGE FILE");
                existingBook.ImageUrl = await UploadFileToStorageAsync(request.ImageFile, request.UserId, "rbooks");
                existingBook.LeftColor = await ExtractLeftSideColorAsync(request.ImageFile);
            }

            // UPDATE SOURCE IF NEW FILE PROVIDED
            if (request.SourceFile is not null)
            {
                logger.LogInformation("UpdateReadingBookCommandHandler -> UPLOADING NEW SOURCE FILE");
                existingBook.SourceUrl = await UploadFileToStorageAsync(request.SourceFile, request.UserId, "rbooks");
            }

            // UPDATE OTHER FIELDS
            existingBook.ReadingId = reading.Id;
            existingBook.Name = request.Name;

            readingBookRepository.Update(existingBook);
            await unitOfWork.CommitAsync();

            logger.LogInformation("UpdateReadingBookCommandHandler -> SUCCESSFULLY UPDATED READING BOOK WITH ID: {Id}", existingBook.Id);

            // DELETE OLD FILES FROM STORAGE IF URLS CHANGED
            if (request.ImageFile is not null && !string.Equals(oldImageUrl, existingBook.ImageUrl, StringComparison.OrdinalIgnoreCase))
            {
                await DeleteFileFromStorageAsync(request.Id, oldImageUrl);
            }

            if (request.SourceFile is not null && !string.Equals(oldSourceUrl, existingBook.SourceUrl, StringComparison.OrdinalIgnoreCase))
            {
                await DeleteFileFromStorageAsync(request.Id, oldSourceUrl);
            }

            var result = mapper.Map<ReadingBook, ReadingBookDto>(existingBook);
            return ServiceResult<ReadingBookDto>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UpdateReadingBookCommandHandler -> ERROR UPDATING READING BOOK");
            return ServiceResult<ReadingBookDto>.Fail("ERROR UPDATING READING BOOK", HttpStatusCode.InternalServerError);
        }
    }

    #region PRIVATE METHODS

    private async Task<string> UploadFileToStorageAsync(IFileUpload file, string userId, string folderName)
    {
        var fileName = $"{userId}/{folderName}/{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}_{file.FileName}";
        
        using var stream = file.OpenReadStream();
        var fileUrl = await storageService.UploadAsync(stream, fileName, file.ContentType, null);
        
        logger.LogInformation("UpdateReadingBookCommandHandler -> SUCCESSFULLY UPLOADED FILE TO STORAGE: {FileUrl}", fileUrl);
        
        return fileUrl;
    }

    private async Task DeleteFileFromStorageAsync(int readingBookId, string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl)) return;

        try
        {
            var fileExists = await storageService.ExistsAsync(fileUrl);

            if (!fileExists) return;

            await storageService.DeleteAsync(fileUrl);
            logger.LogInformation("UpdateReadingBookCommandHandler -> SUCCESSFULLY DELETED FILE FROM STORAGE: {FileUrl}", fileUrl);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "UpdateReadingBookCommandHandler -> FAILED TO DELETE FILE FROM STORAGE FOR READING BOOK {Id}: {FileUrl}", readingBookId, fileUrl);
        }
    }

    private async Task<string> ExtractLeftSideColorAsync(IFileUpload imageFile)
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
            
            logger.LogInformation("UpdateReadingBookCommandHandler -> EXTRACTED COLOR: {Color}", hexColor);
            
            return hexColor;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "UpdateReadingBookCommandHandler -> FAILED TO EXTRACT COLOR, USING DEFAULT");
            return "#CCCCCC"; // DEFAULT GRAY COLOR
        }
    }

    private async Task<ServiceResult<Reading>> VerifyOrCreateReadingAsync(int readingId, string userId, int languageId)
    {
        var reading = await readingRepository.GetByIdAsync(readingId);

        if (reading is not null)
        {
            return ServiceResult<Reading>.Success(reading);
        }

        logger.LogWarning("UpdateReadingBookCommandHandler -> READING NOT FOUND WITH ID: {ReadingId}", readingId);

        var language = await languageRepository.GetByIdAsync(languageId);

        if (language is null)
        {
            logger.LogWarning("UpdateReadingBookCommandHandler -> LANGUAGE NOT FOUND FOR ID: {LanguageId}", languageId);
            return ServiceResult<Reading>.Fail("LANGUAGE NOT FOUND", HttpStatusCode.NotFound);
        }

        var practice = await practiceRepository.ExistsByLanguageIdAsync(languageId);

        if (practice is null)
        {
            logger.LogWarning("UpdateReadingBookCommandHandler -> PRACTICE NOT FOUND FOR LANGUAGE ID: {LanguageId}", languageId);
            return ServiceResult<Reading>.Fail("PRACTICE NOT FOUND FOR LANGUAGE", HttpStatusCode.NotFound);
        }

        reading = new Reading
        {
            UserId = userId,
            LanguageId = languageId,
            PracticeId = practice.Id,
            Language = language,
            Practice = practice
        };

        await readingRepository.CreateAsync(reading);

        logger.LogInformation("UpdateReadingBookCommandHandler -> NEW READING CREATED WITH ID: {ReadingId}", reading.Id);

        return ServiceResult<Reading>.Success(reading);
    }

    #endregion
}
