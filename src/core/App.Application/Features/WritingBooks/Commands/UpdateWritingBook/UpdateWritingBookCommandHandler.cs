using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.WritingBooks.CacheKeys;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.WritingBooks.Commands.UpdateWritingBook;

/// <summary>
/// HANDLER FOR UPDATE WRITING BOOK COMMAND.
/// </summary>
public class UpdateWritingBookCommandHandler(

    IWritingBookRepository writingBookRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateWritingBookCommandHandler> logger,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService,
    IImageProcessingService imageProcessingService,
    IFileStorageHelper fileStorageHelper

) : ICommandHandler<UpdateWritingBookCommand, ServiceResult<int>>
{

    public async Task<ServiceResult<int>> Handle(

        UpdateWritingBookCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateWritingBookCommandHandler -> UPDATING WRITING BOOK WITH ID: {Id}", request.Request.Id);

        // VERIFY WRITING BOOK EXISTS
        var existingBook = await writingBookRepository.GetByIdAsync(request.Request.Id);

        // FAST FAIL
        if (existingBook is null)
        {
            logger.LogWarning("UpdateWritingBookCommandHandler -> WRITING BOOK NOT FOUND WITH ID: {Id}", request.Request.Id);
            return ServiceResult<int>.Fail("WRITING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        // VERIFY OR CREATE WRITING
        var writingResult = await entityVerificationService.VerifyOrCreateWritingAsync(
            request.Request.WritingId, 
            request.Request.UserId, 
            request.Request.LanguageId);

        // FAST FAIL
        if (!writingResult.IsSuccess)
        {
            return ServiceResult<int>.Fail(writingResult.ErrorMessage!, writingResult.Status);
        }

        var writing = writingResult.Data!;

        try
        {
            // STORE OLD FILE URLS FOR DELETION
            var oldImageUrl = existingBook.ImageUrl;
            var oldSourceUrl = existingBook.SourceUrl;

            // UPDATE IMAGE IF NEW FILE PROVIDED
            if (request.Request.ImageFile is not null)
            {
                logger.LogInformation("UpdateWritingBookCommandHandler -> UPLOADING NEW IMAGE FILE");
                existingBook.ImageUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.ImageFile, request.Request.UserId, "wbooks");
                existingBook.LeftColor = await imageProcessingService.ExtractLeftSideColorAsync(request.Request.ImageFile);
            }

            // UPDATE SOURCE IF NEW FILE PROVIDED
            if (request.Request.SourceFile is not null)
            {
                logger.LogInformation("UpdateWritingBookCommandHandler -> UPLOADING NEW SOURCE FILE");
                existingBook.SourceUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.SourceFile, request.Request.UserId, "wbooks");
            }

            // UPDATE OTHER FIELDS
            existingBook.WritingId = writing.Id;
            existingBook.Name = request.Request.Name;

            writingBookRepository.Update(existingBook);
            await unitOfWork.CommitAsync();

            // CACHE INVALIDATION
            await cacheManager.RemoveByPrefixAsync(WritingBookCacheKeys.Prefix);

            logger.LogInformation("UpdateWritingBookCommandHandler -> SUCCESSFULLY UPDATED WRITING BOOK WITH ID: {Id}", existingBook.Id);

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
            logger.LogError(ex, "UpdateWritingBookCommandHandler -> ERROR UPDATING WRITING BOOK");
            return ServiceResult<int>.Fail("ERROR UPDATING WRITING BOOK", HttpStatusCode.InternalServerError);
        }
    }

}
