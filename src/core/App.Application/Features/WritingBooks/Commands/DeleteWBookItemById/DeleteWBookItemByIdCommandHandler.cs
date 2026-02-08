using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.WritingBooks.CacheKeys;
using App.Domain.Exceptions;

namespace App.Application.Features.WritingBooks.Commands.DeleteWBookItemById;

/// <summary>
/// HANDLER FOR DELETE WRITING BOOK BY ID COMMAND.
/// </summary>
public class DeleteWBookItemByIdCommandHandler(

    IWritingBookRepository writingBookRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager,
    IFileStorageHelper fileStorageHelper

    ) : ICommandHandler<DeleteWBookItemByIdCommand, ServiceResult>
{

    public async Task<ServiceResult> Handle(

        DeleteWBookItemByIdCommand request,
        CancellationToken cancellationToken)
    {

        // GET WRITING BOOK
        var writingBook = await writingBookRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("WRITING BOOK NOT FOUND");

        // STORE FILE PATHS BEFORE DELETION
        var imageUrl = writingBook.ImageUrl;
        var sourceUrl = writingBook.SourceUrl;

        await writingBookRepository.RemoveAsync(writingBook.Id);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(WritingBookCacheKeys.Prefix);

        // DELETE FILES FROM STORAGE AFTER DATABASE DELETION
        await fileStorageHelper.DeleteFileFromStorageAsync(imageUrl);
        await fileStorageHelper.DeleteFileFromStorageAsync(sourceUrl);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
