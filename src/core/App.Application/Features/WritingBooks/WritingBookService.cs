using App.Application.Common;
using App.Application.Contracts.Infrastructure.Files;
using App.Application.Contracts.Infrastructure.Storage;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingBooks.Dtos;
using App.Domain.Entities.WritingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Net;

namespace App.Application.Features.WritingBooks;

/// <summary>
/// SERVICE IMPLEMENTATION FOR WRITING BOOK OPERATIONS.
/// </summary>
public class WritingBookService(

    IWritingBookRepository writingBookRepository,
    IWritingRepository writingRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IStorageService storageService,
    ILogger<WritingBookService> logger

    ) : IWritingBookService
{

    #region UTILS

    /// <summary>
    /// UPLOAD FILE TO STORAGE
    /// </summary>
    private async Task<string> UploadFileToStorageAsync(IFileUpload file, string userId, string folderName)
    {
        var fileName = $"{userId}/{folderName}/{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}_{file.FileName}";

        using var stream = file.OpenReadStream();
        var fileUrl = await storageService.UploadAsync(stream, fileName, file.ContentType, null);

        logger.LogInformation("WritingBookService:UploadFileToStorageAsync -> SUCCESSFULLY UPLOADED FILE TO STORAGE: {FileUrl}", fileUrl);

        return fileUrl;
    }

    /// <summary>
    /// DELETE FILE FROM STORAGE
    /// </summary>
    private async Task DeleteFileFromStorageAsync(int writingBookId, string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl)) return;

        try
        {
            var fileExists = await storageService.ExistsAsync(fileUrl);

            if (!fileExists) return;

            await storageService.DeleteAsync(fileUrl);
            logger.LogInformation("WritingBookService:DeleteFileFromStorageAsync -> SUCCESSFULLY DELETED FILE FROM STORAGE: {FileUrl}", fileUrl);

        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "WritingBookService:DeleteFileFromStorageAsync -> FAILED TO DELETE FILE FROM STORAGE FOR WRITING BOOK {Id}: {FileUrl}", writingBookId, fileUrl);
        }
    }

    /// <summary>
    /// EXTRACT LEFT SIDE COLOR FROM IMAGE
    /// </summary>
    private async Task<string> ExtractLeftSideColorAsync(IFileUpload imageFile)
    {
        try
        {
            using var stream = imageFile.OpenReadStream();
            using var image = await Image.LoadAsync<Rgba32>(stream);

            // RESIZE IMAGE FOR PERFORMANCE
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(100, 100),
                Mode = ResizeMode.Max
            }));

            // GET LEFT SIDE PIXELS (FIRST 10 COLUMNS)
            var leftPixels = new List<Rgba32>();
            for (var x = 0; x < Math.Min(10, image.Width); x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    leftPixels.Add(image[x, y]);
                }
            }

            // CALCULATE AVERAGE COLOR
            var avgR = (int)leftPixels.Average(p => p.R);
            var avgG = (int)leftPixels.Average(p => p.G);
            var avgB = (int)leftPixels.Average(p => p.B);

            var hexColor = $"#{avgR:X2}{avgG:X2}{avgB:X2}";

            logger.LogInformation("WritingBookService:ExtractLeftSideColorAsync -> EXTRACTED COLOR: {Color}", hexColor);

            return hexColor;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "WritingBookService:ExtractLeftSideColorAsync -> FAILED TO EXTRACT COLOR, USING DEFAULT");
            return "#CCCCCC"; // DEFAULT GRAY COLOR
        }
    }

    /// <summary>
    /// VERIFY OR CREATE WRITING IF IT DOESN'T EXIST
    /// </summary>
    private async Task<ServiceResult<Writing>> VerifyOrCreateWritingAsync(int writingId, string userId, int languageId)
    {
        var writing = await writingRepository.GetByIdAsync(writingId);

        if (writing is not null)
        {
            return ServiceResult<Writing>.Success(writing);
        }

        logger.LogWarning("WritingBookService:VerifyOrCreateWritingAsync -> WRITING NOT FOUND WITH ID: {WritingId}", writingId);

        var language = await languageRepository.GetByIdAsync(languageId);

        if (language is null)
        {
            logger.LogWarning("WritingBookService:VerifyOrCreateWritingAsync -> LANGUAGE NOT FOUND FOR ID: {LanguageId}", languageId);
            return ServiceResult<Writing>.Fail("LANGUAGE NOT FOUND", HttpStatusCode.NotFound);
        }

        var practice = await practiceRepository.ExistsByLanguageIdAsync(languageId);

        if (practice is null)
        {
            logger.LogWarning("WritingBookService:VerifyOrCreateWritingAsync -> PRACTICE NOT FOUND FOR LANGUAGE ID: {LanguageId}", languageId);
            return ServiceResult<Writing>.Fail("PRACTICE NOT FOUND FOR LANGUAGE", HttpStatusCode.NotFound);
        }

        writing = new Writing
        {
            UserId = userId,
            LanguageId = languageId,
            PracticeId = practice.Id,
            Language = language,
            Practice = practice
        };

        await writingRepository.CreateAsync(writing);

        logger.LogInformation("WritingBookService:VerifyOrCreateWritingAsync -> NEW WRITING CREATED WITH ID: {WritingId}", writing.Id);

        return ServiceResult<Writing>.Success(writing);
    }

    #endregion

    public async Task<ServiceResult<WritingBookWithLanguageId>> GetWritingBookItemByIdAsync(int id)
    {
        logger.LogInformation("WritingBookService:GetWritingBookItemByIdAsync --> FETCHING WRITING BOOK WITH ID: {Id}", id);

        var writingBook = await writingBookRepository.GetWritingBookItemByIdAsync(id);

        if (writingBook is null)
        {
            logger.LogWarning("WritingBookService:GetWritingBookItemByIdAsync --> WRITING BOOK NOT FOUND WITH ID: {Id}", id);
            return ServiceResult<WritingBookWithLanguageId>.Fail("WRITING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        logger.LogInformation("WritingBookService:GetWritingBookItemByIdAsync --> SUCCESSFULLY FETCHED WRITING BOOK: {BookName}", writingBook.Name);

        var result = mapper.Map<WritingBook, WritingBookWithLanguageId>(writingBook);
        return ServiceResult<WritingBookWithLanguageId>.Success(result);
    }

    public async Task<ServiceResult<PagedResult<WritingBookWithTotalCount>>> GetAllWBooksWithPagingAsync(string userId, PagedRequest request)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            logger.LogWarning("WritingBookService:GetAllWBooksWithPagingAsync --> USER ID IS REQUIRED FOR FETCHING WRITING BOOKS");
            return ServiceResult<PagedResult<WritingBookWithTotalCount>>.Fail("USER ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("WritingBookService:GetAllWBooksWithPagingAsync --> FETCHING WRITING BOOKS FOR USER: {UserId}, PAGE: {Page}, PAGE SIZE: {PageSize}", userId, request.Page, request.PageSize);

        var (items, totalCount) = await writingBookRepository.GetAllWBooksWithPagingAsync(userId, request.Page, request.PageSize);

        logger.LogInformation("WritingBookService:GetAllWBooksWithPagingAsync --> FETCHED {Count} WRITING BOOKS OUT OF {Total} FOR USER: {UserId}", items.Count, totalCount, userId);

        var mappedDtos = mapper.Map<List<WritingBook>, List<WritingBookDto>>(items);
        var mappedResult = new WritingBookWithTotalCount
        {
            WritingBookDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<WritingBookWithTotalCount>.Create([mappedResult], request, totalCount);

        return ServiceResult<PagedResult<WritingBookWithTotalCount>>.Success(result);
    }

    public async Task<ServiceResult<List<WritingBookDto>>> GetWBookCreateItemsAsync(string userId, string language, string practice)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(userId))
        {
            logger.LogWarning("WritingBookService:GetWBookCreateItemsAsync --> USER ID IS REQUIRED FOR FETCHING CREATE ITEMS");
            return ServiceResult<List<WritingBookDto>>.Fail("USER ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("WritingBookService:GetWBookCreateItemsAsync --> FETCHING WRITING BOOK CREATE ITEMS FOR USER: {UserId}", userId);

        // CHECK IF LANGUAGES EXIST
        var languageExists = await languageRepository.ExistsByNameAsync(language);

        if (languageExists is null)
        {
            logger.LogWarning("WritingBookService:GetWBookCreateItemsAsync --> LANGUAGE NOT FOUND: {Language}", language);
            return ServiceResult<List<WritingBookDto>>.Fail($"LANGUAGE '{language}' NOT FOUND",
                HttpStatusCode.NotFound);
        }

        // CHECK IF PRACTICE EXIST
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(practice, languageExists.Id);

        if (practiceExists is null)
        {
            logger.LogWarning("WritingBookService:GetWBookCreateItemsAsync --> PRACTICE NOT FOUND: {Practice} FOR LANGUAGE: {Language}", practice, language);
            return ServiceResult<List<WritingBookDto>>.Fail($"PRACTICE '{practice}' NOT FOUND FOR LANGUAGE '{language}'.",
                HttpStatusCode.NotFound);
        }

        var writingBooks = await writingBookRepository.GetWBookCreateItemsAsync(userId, languageExists.Id, practiceExists.Id);

        var result = mapper.Map<List<WritingBook>, List<WritingBookDto>>(writingBooks);

        logger.LogInformation("WritingBookService:GetWBookCreateItemsAsync -> SUCCESSFULLY FETCHED {Count} CREATE ITEMS", result.Count);

        return ServiceResult<List<WritingBookDto>>.Success(result);
    }

    public async Task<ServiceResult> DeleteWBookItemByIdAsync(int id)
    {
        // GUARD CLAUSE
        if (id <= 0)
        {
            logger.LogWarning("WritingBookService:DeleteWBookItemByIdAsync -> INVALID WRITING BOOK ID FOR DELETION: {Id}", id);
            return ServiceResult.Fail("INVALID WRITING BOOK ID", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("WritingBookService:DeleteWBookItemByIdAsync -> ATTEMPTING TO DELETE WRITING BOOK WITH ID: {Id}", id);

        var writingBook = await writingBookRepository.GetByIdAsync(id);

        // FAST FAIL
        if (writingBook is null)
        {
            logger.LogWarning("WritingBookService:DeleteWBookItemByIdAsync -> WRITING BOOK NOT FOUND FOR DELETION WITH ID: {Id}", id);
            return ServiceResult.Fail("WRITING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        // STORE FILE PATHS BEFORE DELETION
        var imageUrl = writingBook.ImageUrl;
        var sourceUrl = writingBook.SourceUrl;

        writingBookRepository.Delete(writingBook);
        await unitOfWork.CommitAsync();

        logger.LogInformation("WritingBookService:DeleteWBookItemByIdAsync -> SUCCESSFULLY DELETED WRITING BOOK FROM DATABASE WITH ID: {Id}", id);

        // DELETE FILES FROM STORAGE AFTER DATABASE DELETION
        await DeleteFileFromStorageAsync(id, imageUrl);
        await DeleteFileFromStorageAsync(id, sourceUrl);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult<WritingBookDto>> WritingBookAddAsync(CreateWritingBookRequest request)
    {
        logger.LogInformation("WritingBookService:WritingBookAddAsync -> CREATING NEW WRITING BOOK FOR WRITING ID: {WritingId}", request.WritingId);

        // VERIFY OR CREATE WRITING
        var writingResult = await VerifyOrCreateWritingAsync(request.WritingId, request.UserId, request.LanguageId);

        // FAST FAIL
        if (!writingResult.IsSuccess)
        {
            return ServiceResult<WritingBookDto>.Fail(writingResult.ErrorMessage!, writingResult.Status);
        }

        var writing = writingResult.Data!;

        try
        {
            // UPLOAD FILES TO STORAGE
            logger.LogInformation("WritingBookService:WritingBookAddAsync -> UPLOADING FILES TO STORAGE");

            var imageUrl = await UploadFileToStorageAsync(request.ImageFile, request.UserId, "wbooks");
            var sourceUrl = await UploadFileToStorageAsync(request.SourceFile, request.UserId, "wbooks");

            // EXTRACT LEFT SIDE COLOR FROM IMAGE
            logger.LogInformation("WritingBookService:WritingBookAddAsync -> EXTRACTING LEFT SIDE COLOR");
            var leftColor = await ExtractLeftSideColorAsync(request.ImageFile);

            // CREATE WRITING BOOK
            var writingBook = new WritingBook
            {
                WritingId = writing.Id,
                Name = request.Name,
                ImageUrl = imageUrl,
                LeftColor = leftColor,
                SourceUrl = sourceUrl,
                Writing = writing
            };

            await writingBookRepository.CreateAsync(writingBook);
            await unitOfWork.CommitAsync();

            logger.LogInformation("WritingBookService:WritingBookAddAsync -> SUCCESSFULLY CREATED WRITING BOOK WITH ID: {Id}, NAME: {Name}", writingBook.Id, writingBook.Name);

            var result = mapper.Map<WritingBook, WritingBookDto>(writingBook);
            return ServiceResult<WritingBookDto>.SuccessAsCreated(result, $"/api/WritingBook/{writingBook.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "WritingBookService:WritingBookAddAsync -> ERROR CREATING WRITING BOOK");
            return ServiceResult<WritingBookDto>.Fail("ERROR CREATING WRITING BOOK", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ServiceResult<WritingBookDto>> WritingBookUpdateAsync(UpdateWritingBookRequest request)
    {
        logger.LogInformation("WritingBookService:WritingBookUpdateAsync -> UPDATING WRITING BOOK WITH ID: {Id}", request.Id);

        // VERIFY WRITING BOOK EXISTS
        var existingBook = await writingBookRepository.GetByIdAsync(request.Id);

        // FAST FAIL
        if (existingBook is null)
        {
            logger.LogWarning("WritingBookService:WritingBookUpdateAsync -> WRITING BOOK NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<WritingBookDto>.Fail("WRITING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        // VERIFY OR CREATE WRITING
        var writingResult = await VerifyOrCreateWritingAsync(request.WritingId, request.UserId, request.LanguageId);

        // FAST FAIL
        if (!writingResult.IsSuccess)
        {
            return ServiceResult<WritingBookDto>.Fail(writingResult.ErrorMessage!, writingResult.Status);
        }

        var writing = writingResult.Data!;

        try
        {
            // STORE OLD FILE URLS FOR DELETION
            var oldImageUrl = existingBook.ImageUrl;
            var oldSourceUrl = existingBook.SourceUrl;

            // UPDATE IMAGE IF NEW FILE PROVIDED
            if (request.ImageFile is not null)
            {
                logger.LogInformation("WritingBookService:WritingBookUpdateAsync -> UPLOADING NEW IMAGE FILE");
                existingBook.ImageUrl = await UploadFileToStorageAsync(request.ImageFile, request.UserId, "wbooks");
                existingBook.LeftColor = await ExtractLeftSideColorAsync(request.ImageFile);
            }

            // UPDATE SOURCE IF NEW FILE PROVIDED
            if (request.SourceFile is not null)
            {
                logger.LogInformation("WritingBookService:WritingBookUpdateAsync -> UPLOADING NEW SOURCE FILE");
                existingBook.SourceUrl = await UploadFileToStorageAsync(request.SourceFile, request.UserId, "wbooks");
            }

            // UPDATE OTHER FIELDS
            existingBook.WritingId = writing.Id;
            existingBook.Name = request.Name;

            writingBookRepository.Update(existingBook);
            await unitOfWork.CommitAsync();

            logger.LogInformation("WritingBookService:WritingBookUpdateAsync -> SUCCESSFULLY UPDATED WRITING BOOK WITH ID: {Id}", existingBook.Id);

            // DELETE OLD FILES FROM STORAGE IF URLS CHANGED
            if (request.ImageFile is not null && !string.Equals(oldImageUrl, existingBook.ImageUrl, StringComparison.OrdinalIgnoreCase))
            {
                await DeleteFileFromStorageAsync(request.Id, oldImageUrl);
            }

            if (request.SourceFile is not null && !string.Equals(oldSourceUrl, existingBook.SourceUrl, StringComparison.OrdinalIgnoreCase))
            {
                await DeleteFileFromStorageAsync(request.Id, oldSourceUrl);
            }

            var result = mapper.Map<WritingBook, WritingBookDto>(existingBook);
            return ServiceResult<WritingBookDto>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "WritingBookService:WritingBookUpdateAsync -> ERROR UPDATING WRITING BOOK");
            return ServiceResult<WritingBookDto>.Fail("ERROR UPDATING WRITING BOOK", HttpStatusCode.InternalServerError);
        }
    }
}
