using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.ReadingBooks.CacheKeys;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.ReadingBooks.Commands.DeleteRBookItemById;

/// <summary>
/// HANDLER FOR DELETE READING BOOK BY ID COMMAND.
/// </summary>
public class DeleteRBookItemByIdCommandHandler(

    IReadingBookRepository readingBookRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteRBookItemByIdCommandHandler> logger,
    IStaticCacheManager cacheManager,
    IFileStorageHelper fileStorageHelper

    ) : ICommandHandler<DeleteRBookItemByIdCommand, ServiceResult>
{

    public async Task<ServiceResult> Handle(

        DeleteRBookItemByIdCommand request, 
        CancellationToken cancellationToken)
    {

        logger.LogInformation("DeleteRBookItemByIdCommandHandler -> ATTEMPTING TO DELETE READING BOOK WITH ID: {Id}", request.Id);

        var readingBook = await readingBookRepository.GetByIdAsync(request.Id);

        // FAST FAIL
        if (readingBook is null)
        {
            logger.LogWarning("DeleteRBookItemByIdCommandHandler -> READING BOOK NOT FOUND FOR DELETION WITH ID: {Id}", request.Id);
            return ServiceResult.Fail("READING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        // STORE FILE PATHS BEFORE DELETION
        var imageUrl = readingBook.ImageUrl;
        var sourceUrl = readingBook.SourceUrl;

        readingBookRepository.Delete(readingBook);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(ReadingBookCacheKeys.Prefix);

        logger.LogInformation("DeleteRBookItemByIdCommandHandler -> SUCCESSFULLY DELETED READING BOOK FROM DATABASE WITH ID: {Id}", request.Id);

        // DELETE FILES FROM STORAGE AFTER DATABASE DELETION
        await fileStorageHelper.DeleteFileFromStorageAsync(imageUrl);
        await fileStorageHelper.DeleteFileFromStorageAsync(sourceUrl);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
