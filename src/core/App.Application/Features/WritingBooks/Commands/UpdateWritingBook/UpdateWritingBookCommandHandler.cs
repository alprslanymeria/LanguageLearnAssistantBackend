using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.WritingBooks.CacheKeys;
using App.Domain.Exceptions;

namespace App.Application.Features.WritingBooks.Commands.UpdateWritingBook;

/// <summary>
/// HANDLER FOR UPDATE WRITING BOOK COMMAND.
/// </summary>
public class UpdateWritingBookCommandHandler(

    IWritingBookRepository writingBookRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager,
    IEntityVerificationService entityVerificationService,
    IImageProcessingService imageProcessingService,
    IFileStorageHelper fileStorageHelper,
    IPracticeRepository practiceRepository

) : ICommandHandler<UpdateWritingBookCommand, ServiceResult>
{

    public async Task<ServiceResult> Handle(

        UpdateWritingBookCommand request,
        CancellationToken cancellationToken)
    {

        // GET PRACTICE
        var practice = await practiceRepository.ExistsByLanguageIdAsync(request.Request.LanguageId)
            ?? throw new NotFoundException("PRACTICE NOT FOUND");

        // VERIFY WRITING BOOK EXISTS
        var existingBook = await writingBookRepository.GetByIdAsync(request.Request.ItemId)
            ?? throw new NotFoundException("WRITING BOOK NOT FOUND");

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

        // STORE OLD FILE URLS FOR DELETION
        var oldImageUrl = existingBook.ImageUrl;
        var oldSourceUrl = existingBook.SourceUrl;

        // UPDATE IMAGE IF NEW FILE PROVIDED
        if (request.Request.ImageFile is not null)
        {
            existingBook.ImageUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.ImageFile, request.Request.UserId, "wbooks");
            existingBook.LeftColor = await imageProcessingService.ExtractLeftSideColorAsync(request.Request.ImageFile);
        }

        // UPDATE SOURCE IF NEW FILE PROVIDED
        if (request.Request.SourceFile is not null)
        {
            existingBook.SourceUrl = await fileStorageHelper.UploadFileToStorageAsync(request.Request.SourceFile, request.Request.UserId, "wbooks");
        }

        // UPDATE OTHER FIELDS
        existingBook.WritingId = writing.Id;
        existingBook.Name = request.Request.BookName;

        writingBookRepository.Update(existingBook);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(WritingBookCacheKeys.Prefix);

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
