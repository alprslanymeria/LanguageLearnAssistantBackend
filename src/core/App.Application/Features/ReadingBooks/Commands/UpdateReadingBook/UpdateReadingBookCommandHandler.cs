using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.ReadingBooks.CacheKeys;
using App.Domain.Exceptions;

namespace App.Application.Features.ReadingBooks.Commands.UpdateReadingBook;

/// <summary>
/// HANDLER FOR UPDATE READING BOOK COMMAND.
/// </summary>
public class UpdateReadingBookCommandHandler(

    IReadingBookRepository readingBookRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService,
    IImageProcessingService imageProcessingService,
    IFileStorageHelper fileStorageHelper,
    IPracticeRepository practiceRepository

    ) : ICommandHandler<UpdateReadingBookCommand, ServiceResult>
{

    public async Task<ServiceResult> Handle(

        UpdateReadingBookCommand request,
        CancellationToken cancellationToken)
    {

        // GET PRACTICE
        var practice = await practiceRepository.ExistsByLanguageIdAsync(request.Request.LanguageId)
            ?? throw new NotFoundException("PRACTICE NOT FOUND");

        // VERIFY READING BOOK EXISTS
        var existingBook = await readingBookRepository.GetByIdAsync(request.Request.ItemId)
            ?? throw new NotFoundException("READING BOOK NOT FOUND");

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

        // STORE OLD FILE URLS FOR DELETION
        var oldImageUrl = existingBook.ImageUrl;
        var oldSourceUrl = existingBook.SourceUrl;

        // UPDATE IMAGE IF NEW FILE PROVIDED
        if (request.Request.ImageFile is not null)
        {
            existingBook.ImageUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.ImageFile, request.Request.UserId, "rbooks");
            existingBook.LeftColor = await imageProcessingService.ExtractLeftSideColorAsync(request.Request.ImageFile);
        }

        // UPDATE SOURCE IF NEW FILE PROVIDED
        if (request.Request.SourceFile is not null)
        {
            existingBook.SourceUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.SourceFile, request.Request.UserId, "rbooks");
        }

        // UPDATE OTHER FIELDS
        existingBook.ReadingId = reading.Id;
        existingBook.Name = request.Request.BookName;

        readingBookRepository.Update(existingBook);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(ReadingBookCacheKeys.Prefix);

        // DELETE OLD FILES FROM STORAGE IF URLS CHANGED
        if (request.Request.ImageFile is not null && !string.Equals(oldImageUrl, existingBook.ImageUrl, StringComparison.OrdinalIgnoreCase))
        {
            await fileStorageHelper.DeleteFileFromStorageAsync(oldImageUrl);
        }

        if (request.Request.SourceFile is not null && !string.Equals(oldSourceUrl, existingBook.SourceUrl, StringComparison.OrdinalIgnoreCase))
        {
            await fileStorageHelper.DeleteFileFromStorageAsync(oldSourceUrl);
        }

        return ServiceResult.Success();
    }
}
