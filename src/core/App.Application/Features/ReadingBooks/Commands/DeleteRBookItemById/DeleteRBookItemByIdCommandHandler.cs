using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.ReadingBooks.CacheKeys;
using App.Domain.Exceptions;

namespace App.Application.Features.ReadingBooks.Commands.DeleteRBookItemById;

/// <summary>
/// HANDLER FOR DELETE READING BOOK BY ID COMMAND.
/// </summary>
public class DeleteRBookItemByIdCommandHandler(

    IReadingBookRepository readingBookRepository,
    IUnitOfWork unitOfWork,
    IStaticCacheManager cacheManager,
    IFileStorageHelper fileStorageHelper

    ) : ICommandHandler<DeleteRBookItemByIdCommand, ServiceResult>
{

    public async Task<ServiceResult> Handle(

        DeleteRBookItemByIdCommand request,
        CancellationToken cancellationToken)
    {

        // GET READING BOOK
        var readingBook = await readingBookRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("READING BOOK NOT FOUND");

        // STORE FILE PATHS BEFORE DELETION
        var imageUrl = readingBook.ImageUrl;
        var sourceUrl = readingBook.SourceUrl;

        await readingBookRepository.RemoveAsync(readingBook.Id);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(ReadingBookCacheKeys.Prefix);

        // DELETE FILES FROM STORAGE AFTER DATABASE DELETION
        await fileStorageHelper.DeleteFileFromStorageAsync(imageUrl);
        await fileStorageHelper.DeleteFileFromStorageAsync(sourceUrl);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
