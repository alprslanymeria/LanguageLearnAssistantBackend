using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Application.Features.WritingBooks.CacheKeys;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.WritingBooks.Commands.DeleteWBookItemById;

/// <summary>
/// HANDLER FOR DELETE WRITING BOOK BY ID COMMAND.
/// </summary>
public class DeleteWBookItemByIdCommandHandler(

    IWritingBookRepository writingBookRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteWBookItemByIdCommandHandler> logger,
    IStaticCacheManager cacheManager,
    IFileStorageHelper fileStorageHelper

    ) : ICommandHandler<DeleteWBookItemByIdCommand, ServiceResult>
{

    public async Task<ServiceResult> Handle(

        DeleteWBookItemByIdCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteWBookItemByIdCommandHandler -> ATTEMPTING TO DELETE WRITING BOOK WITH ID: {Id}", request.Id);

        var writingBook = await writingBookRepository.GetByIdAsync(request.Id);

        // FAST FAIL
        if (writingBook is null)
        {
            logger.LogWarning("DeleteWBookItemByIdCommandHandler -> WRITING BOOK NOT FOUND FOR DELETION WITH ID: {Id}", request.Id);
            return ServiceResult.Fail("WRITING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        // STORE FILE PATHS BEFORE DELETION
        var imageUrl = writingBook.ImageUrl;
        var sourceUrl = writingBook.SourceUrl;

        writingBookRepository.Delete(writingBook);
        await unitOfWork.CommitAsync();

        // CACHE INVALIDATION
        await cacheManager.RemoveByPrefixAsync(WritingBookCacheKeys.Prefix);

        logger.LogInformation("DeleteWBookItemByIdCommandHandler -> SUCCESSFULLY DELETED WRITING BOOK FROM DATABASE WITH ID: {Id}", request.Id);

        // DELETE FILES FROM STORAGE AFTER DATABASE DELETION
        await fileStorageHelper.DeleteFileFromStorageAsync(imageUrl);
        await fileStorageHelper.DeleteFileFromStorageAsync(sourceUrl);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
