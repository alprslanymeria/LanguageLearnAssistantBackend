using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.WritingBooks.CacheKeys;
using App.Domain.Entities.WritingEntities;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.WritingBooks.Commands.CreateWritingBook;

/// <summary>
/// HANDLER FOR CREATE WRITING BOOK COMMAND.
/// </summary>
public class CreateWritingBookCommandHandler(

    IWritingBookRepository writingBookRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateWritingBookCommandHandler> logger,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService,
    IImageProcessingService imageProcessingService,
    IFileStorageHelper fileStorageHelper

    ) : ICommandHandler<CreateWritingBookCommand, ServiceResult<int>>
{

    public async Task<ServiceResult<int>> Handle(

        CreateWritingBookCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateWritingBookCommandHandler -> CREATING NEW WRITING BOOK FOR WRITING ID: {WritingId}", request.Request.WritingId);

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
            // UPLOAD FILES TO STORAGE
            logger.LogInformation("CreateWritingBookCommandHandler -> UPLOADING FILES TO STORAGE");

            var imageUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.ImageFile, request.Request.UserId, "wbooks");
            var sourceUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.SourceFile, request.Request.UserId, "wbooks");

            // EXTRACT LEFT SIDE COLOR FROM IMAGE
            logger.LogInformation("CreateWritingBookCommandHandler -> EXTRACTING LEFT SIDE COLOR");
            var leftColor = await imageProcessingService.ExtractLeftSideColorAsync(request.Request.ImageFile);

            // CREATE WRITING BOOK
            var writingBook = new WritingBook
            {
                WritingId = writing.Id,
                Name = request.Request.Name,
                ImageUrl = imageUrl,
                LeftColor = leftColor,
                SourceUrl = sourceUrl,
                Writing = writing
            };

            await writingBookRepository.CreateAsync(writingBook);
            await unitOfWork.CommitAsync();

            // CACHE INVALIDATION
            await cacheManager.RemoveByPrefixAsync(WritingBookCacheKeys.Prefix);

            logger.LogInformation("CreateWritingBookCommandHandler -> SUCCESSFULLY CREATED WRITING BOOK WITH ID: {Id}, NAME: {Name}", writingBook.Id, writingBook.Name);

            return ServiceResult<int>.SuccessAsCreated(writingBook.Id, $"/api/WritingBook/{writingBook.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateWritingBookCommandHandler -> ERROR CREATING WRITING BOOK");
            return ServiceResult<int>.Fail("ERROR CREATING WRITING BOOK", HttpStatusCode.InternalServerError);
        }
    }
}
