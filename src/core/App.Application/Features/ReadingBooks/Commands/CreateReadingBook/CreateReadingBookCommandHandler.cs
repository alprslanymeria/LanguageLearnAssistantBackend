using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.ReadingBooks.CacheKeys;
using App.Domain.Entities.ReadingEntities;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ReadingBooks.Commands.CreateReadingBook;

/// <summary>
/// HANDLER FOR CREATE READING BOOK COMMAND.
/// </summary>
public class CreateReadingBookCommandHandler(

    IReadingBookRepository readingBookRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateReadingBookCommandHandler> logger,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService,
    IImageProcessingService imageProcessingService,
    IFileStorageHelper fileStorageHelper

    ) : ICommandHandler<CreateReadingBookCommand, ServiceResult<int>>
{

    public async Task<ServiceResult<int>> Handle(

        CreateReadingBookCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateReadingBookCommandHandler -> CREATING NEW READING BOOK FOR READING ID: {ReadingId}", request.Request.ReadingId);

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
            // UPLOAD FILES TO STORAGE
            logger.LogInformation("CreateReadingBookCommandHandler -> UPLOADING FILES TO STORAGE");

            var imageUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.ImageFile, request.Request.UserId, "rbooks");
            var sourceUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.SourceFile, request.Request.UserId, "rbooks");

            // EXTRACT LEFT SIDE COLOR FROM IMAGE
            logger.LogInformation("CreateReadingBookCommandHandler -> EXTRACTING LEFT SIDE COLOR");
            var leftColor = await imageProcessingService.ExtractLeftSideColorAsync(request.Request.ImageFile);

            // CREATE READING BOOK
            var readingBook = new ReadingBook
            {
                ReadingId = reading.Id,
                Name = request.Request.Name,
                ImageUrl = imageUrl,
                LeftColor = leftColor,
                SourceUrl = sourceUrl,
                Reading = reading
            };

            await readingBookRepository.CreateAsync(readingBook);
            await unitOfWork.CommitAsync();

            // CACHE INVALIDATION
            await cacheManager.RemoveByPrefixAsync(ReadingBookCacheKeys.Prefix);

            logger.LogInformation("CreateReadingBookCommandHandler -> SUCCESSFULLY CREATED READING BOOK WITH ID: {Id}, NAME: {Name}", readingBook.Id, readingBook.Name);

            return ServiceResult<int>.SuccessAsCreated(readingBook.Id, $"/api/ReadingBook/{readingBook.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateReadingBookCommandHandler -> ERROR CREATING READING BOOK");
            return ServiceResult<int>.Fail("ERROR CREATING READING BOOK", HttpStatusCode.InternalServerError);
        }
    }

}
