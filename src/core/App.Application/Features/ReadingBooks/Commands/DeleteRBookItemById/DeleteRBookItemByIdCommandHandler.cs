using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Storage;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.ReadingBooks.Commands.DeleteRBookItemById;

/// <summary>
/// HANDLER FOR DELETE READING BOOK BY ID COMMAND.
/// </summary>
public class DeleteRBookItemByIdCommandHandler(
    IReadingBookRepository readingBookRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService,
    ILogger<DeleteRBookItemByIdCommandHandler> logger
    ) : ICommandHandler<DeleteRBookItemByIdCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        DeleteRBookItemByIdCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteRBookItemByIdCommandHandler -> ATTEMPTING TO DELETE READING BOOK WITH ID: {Id}", request.Id);

        var readingBook = await readingBookRepository.GetByIdAsync(request.Id);

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

        logger.LogInformation("DeleteRBookItemByIdCommandHandler -> SUCCESSFULLY DELETED READING BOOK FROM DATABASE WITH ID: {Id}", request.Id);

        // DELETE FILES FROM STORAGE AFTER DATABASE DELETION
        await DeleteFileFromStorageAsync(request.Id, imageUrl);
        await DeleteFileFromStorageAsync(request.Id, sourceUrl);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    private async Task DeleteFileFromStorageAsync(int readingBookId, string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl)) return;

        try
        {
            var fileExists = await storageService.ExistsAsync(fileUrl);

            if (!fileExists) return;

            await storageService.DeleteAsync(fileUrl);
            logger.LogInformation("DeleteRBookItemByIdCommandHandler -> SUCCESSFULLY DELETED FILE FROM STORAGE: {FileUrl}", fileUrl);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "DeleteRBookItemByIdCommandHandler -> FAILED TO DELETE FILE FROM STORAGE FOR READING BOOK {Id}: {FileUrl}", readingBookId, fileUrl);
        }
    }
}
