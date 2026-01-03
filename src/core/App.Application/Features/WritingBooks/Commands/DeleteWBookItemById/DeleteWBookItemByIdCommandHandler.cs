using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Storage;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.WritingBooks.Commands.DeleteWBookItemById;

/// <summary>
/// HANDLER FOR DELETE WRITING BOOK BY ID COMMAND.
/// </summary>
public class DeleteWBookItemByIdCommandHandler(
    IWritingBookRepository writingBookRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService,
    ILogger<DeleteWBookItemByIdCommandHandler> logger
    ) : ICommandHandler<DeleteWBookItemByIdCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        DeleteWBookItemByIdCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteWBookItemByIdCommandHandler -> ATTEMPTING TO DELETE WRITING BOOK WITH ID: {Id}", request.Id);

        var writingBook = await writingBookRepository.GetByIdAsync(request.Id);

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

        logger.LogInformation("DeleteWBookItemByIdCommandHandler -> SUCCESSFULLY DELETED WRITING BOOK FROM DATABASE WITH ID: {Id}", request.Id);

        // DELETE FILES FROM STORAGE AFTER DATABASE DELETION
        await DeleteFileFromStorageAsync(request.Id, imageUrl);
        await DeleteFileFromStorageAsync(request.Id, sourceUrl);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    private async Task DeleteFileFromStorageAsync(int writingBookId, string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl)) return;

        try
        {
            var fileExists = await storageService.ExistsAsync(fileUrl);

            if (!fileExists) return;

            await storageService.DeleteAsync(fileUrl);
            logger.LogInformation("DeleteWBookItemByIdCommandHandler -> SUCCESSFULLY DELETED FILE FROM STORAGE: {FileUrl}", fileUrl);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "DeleteWBookItemByIdCommandHandler -> FAILED TO DELETE FILE FROM STORAGE FOR WRITING BOOK {Id}: {FileUrl}", writingBookId, fileUrl);
        }
    }
}
