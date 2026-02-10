using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.ReadingBooks.CacheKeys;
using App.Domain.Entities.ReadingEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.ReadingBooks.Commands.CreateReadingBook;

/// <summary>
/// HANDLER FOR CREATE READING BOOK COMMAND.
/// </summary>
public class CreateReadingBookCommandHandler(

    IReadingBookRepository readingBookRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService,
    IImageProcessingService imageProcessingService,
    IFileStorageHelper fileStorageHelper,
    IPracticeRepository practiceRepository

    ) : ICommandHandler<CreateReadingBookCommand, ServiceResult>
{

    public async Task<ServiceResult> Handle(

        CreateReadingBookCommand request,
        CancellationToken cancellationToken)
    {

        // GET PRACTICE
        var practice = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Request.Practice, request.Request.LanguageId)
            ?? throw new NotFoundException("PRACTICE NOT FOUND");

        // VERIFY OR CREATE READING
        var readingResult = await entityVerificationService.VerifyOrCreateReadingAsync(

            practice.Id,
            request.Request.UserId,
            request.Request.LanguageId);

        if (readingResult.IsFail)
        {
            throw new BusinessException(readingResult.ErrorMessage!.First(), readingResult.Status);
        }

        var reading = readingResult.Data!;

        // UPLOAD FILES TO STORAGE
        var imageUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.ImageFile, request.Request.UserId, "rbooks");
        var sourceUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.SourceFile, request.Request.UserId, "rbooks");

        // EXTRACT LEFT SIDE COLOR FROM IMAGE
        var leftColor = await imageProcessingService.ExtractLeftSideColorAsync(request.Request.ImageFile);

        // CREATE READING BOOK
        var readingBook = new ReadingBook
        {
            Name = request.Request.BookName,
            ImageUrl = imageUrl,
            LeftColor = leftColor,
            SourceUrl = sourceUrl,
            ReadingId = reading.Id
        };

        await readingBookRepository.AddAsync(readingBook);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(ReadingBookCacheKeys.Prefix);

        return ServiceResult.Success(HttpStatusCode.Created);
    }

}
