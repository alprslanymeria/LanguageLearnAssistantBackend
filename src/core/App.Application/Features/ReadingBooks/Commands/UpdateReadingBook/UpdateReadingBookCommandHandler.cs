using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.ReadingBooks.CacheKeys;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ReadingBooks.Commands.UpdateReadingBook;

/// <summary>
/// HANDLER FOR UPDATE READING BOOK COMMAND.
/// </summary>
public class UpdateReadingBookCommandHandler(

    IReadingBookRepository readingBookRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateReadingBookCommandHandler> logger,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService,
    IImageProcessingService imageProcessingService,
    IFileStorageHelper fileStorageHelper

    ) : ICommandHandler<UpdateReadingBookCommand, ServiceResult<int>>
{

    public async Task<ServiceResult<int>> Handle(

        UpdateReadingBookCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateReadingBookCommandHandler -> UPDATING READING BOOK WITH ID: {Id}", request.Request.Id);

        // VERIFY READING BOOK EXISTS
        var existingBook = await readingBookRepository.GetByIdAsync(request.Request.Id);

        // FAST FAIL
        if (existingBook is null)
        {
            logger.LogWarning("UpdateReadingBookCommandHandler -> READING BOOK NOT FOUND WITH ID: {Id}", request.Request.Id);
            return ServiceResult<int>.Fail("READING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        // VERIFY OR CREATE READING
        var readingResult = await entityVerificationService.VerifyOrCreateReadingAsync(
            request.Request.ReadingId, 
            request.Request.UserId, 
            request.Request.LanguageId);

        // FAST FAIL
        if (!readingResult.IsSuccess)
        {
            return ServiceResult<int>.Fail(readingResult.ErrorMessage!, readingResult.Status);
        }

        var reading = readingResult.Data!;

        try
        {
            // STORE OLD FILE URLS FOR DELETION
            var oldImageUrl = existingBook.ImageUrl;
            var oldSourceUrl = existingBook.SourceUrl;

            // UPDATE IMAGE IF NEW FILE PROVIDED
            if (request.Request.ImageFile is not null)
            {
                logger.LogInformation("UpdateReadingBookCommandHandler -> UPLOADING NEW IMAGE FILE");
                existingBook.ImageUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.ImageFile, request.Request.UserId, "rbooks");
                existingBook.LeftColor = await imageProcessingService.ExtractLeftSideColorAsync(request.Request.ImageFile);
            }

            // UPDATE SOURCE IF NEW FILE PROVIDED
            if (request.Request.SourceFile is not null)
            {
                logger.LogInformation("UpdateReadingBookCommandHandler -> UPLOADING NEW SOURCE FILE");
                existingBook.SourceUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.SourceFile, request.Request.UserId, "rbooks");
            }

            // UPDATE OTHER FIELDS
            existingBook.ReadingId = reading.Id;
            existingBook.Name = request.Request.Name;

            readingBookRepository.Update(existingBook);
            await unitOfWork.CommitAsync();

            // CACHE INVALIDATION
            await cacheManager.RemoveByPrefixAsync(ReadingBookCacheKeys.Prefix);

            logger.LogInformation("UpdateReadingBookCommandHandler -> SUCCESSFULLY UPDATED READING BOOK WITH ID: {Id}", existingBook.Id);

            // DELETE OLD FILES FROM STORAGE IF URLS CHANGED
            if (request.Request.ImageFile is not null && !string.Equals(oldImageUrl, existingBook.ImageUrl, StringComparison.OrdinalIgnoreCase))
            {
                await fileStorageHelper.DeleteFileFromStorageAsync(oldImageUrl);
            }

            if (request.Request.SourceFile is not null && !string.Equals(oldSourceUrl, existingBook.SourceUrl, StringComparison.OrdinalIgnoreCase))
            {
                await fileStorageHelper.DeleteFileFromStorageAsync(oldSourceUrl);
            }

            return ServiceResult<int>.Success(existingBook.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UpdateReadingBookCommandHandler -> ERROR UPDATING READING BOOK");
            return ServiceResult<int>.Fail("ERROR UPDATING READING BOOK", HttpStatusCode.InternalServerError);
        }
    }
}
