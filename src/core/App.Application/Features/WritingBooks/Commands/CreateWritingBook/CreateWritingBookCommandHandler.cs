using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.WritingBooks.CacheKeys;
using App.Domain.Entities.WritingEntities;
using App.Domain.Exceptions;

namespace App.Application.Features.WritingBooks.Commands.CreateWritingBook;

/// <summary>
/// HANDLER FOR CREATE WRITING BOOK COMMAND.
/// </summary>
public class CreateWritingBookCommandHandler(

    IWritingBookRepository writingBookRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService,
    IImageProcessingService imageProcessingService,
    IFileStorageHelper fileStorageHelper,
    IPracticeRepository practiceRepository

    ) : ICommandHandler<CreateWritingBookCommand, ServiceResult>
{

    public async Task<ServiceResult> Handle(

        CreateWritingBookCommand request,
        CancellationToken cancellationToken)
    {
        var practice = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Request.Practice, request.Request.LanguageId)
            ?? throw new NotFoundException("PRACTICE NOT FOUND");

        // VERIFY OR CREATE WRITING
        var writingResult = await entityVerificationService.VerifyOrCreateWritingAsync(

            practice.Id,
            request.Request.UserId,
            request.Request.LanguageId);

        if (writingResult.IsFail)
        {
            throw new BusinessException(writingResult.ErrorMessage!.First(), writingResult.Status);
        }

        var writing = writingResult.Data!;

        // UPLOAD FILES TO STORAGE
        var imageUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.ImageFile, request.Request.UserId, "wbooks");
        var sourceUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.SourceFile, request.Request.UserId, "wbooks");

        // EXTRACT LEFT SIDE COLOR FROM IMAGE
        var leftColor = await imageProcessingService.ExtractLeftSideColorAsync(request.Request.ImageFile);

        // CREATE WRITING BOOK
        var writingBook = new WritingBook
        {
            Name = request.Request.BookName,
            ImageUrl = imageUrl,
            LeftColor = leftColor,
            SourceUrl = sourceUrl,
            WritingId = writing.Id
        };

        await writingBookRepository.AddAsync(writingBook);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(WritingBookCacheKeys.Prefix);

        return ServiceResult.Success(HttpStatusCode.Created);
    }
}
