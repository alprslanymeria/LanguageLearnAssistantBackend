using App.Application.Common;
using App.Application.Contracts.Infrastructure.Files;
using App.Application.Contracts.Infrastructure.Storage;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingBooks.Dtos;
using App.Domain.Entities.ReadingEntities;
using Microsoft.Extensions.Logging;
using System.Net;
using MapsterMapper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace App.Application.Features.ReadingBooks;

/// <summary>
/// SERVICE IMPLEMENTATION FOR READING BOOK OPERATIONS.
/// </summary>
public class ReadingBookService(

    IReadingBookRepository readingBookRepository,
    IReadingRepository readingRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IStorageService storageService,
    ILogger<ReadingBookService> logger
    
    ) : IReadingBookService
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
        
        logger.LogInformation("ReadingBookService:UploadFileToStorageAsync -> SUCCESSFULLY UPLOADED FILE TO STORAGE: {FileUrl}", fileUrl);
        
        return fileUrl;
    }

    /// <summary>
    /// DELETE FILE FROM STORAGE
    /// </summary>
    private async Task DeleteFileFromStorageAsync(int readingBookId, string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl)) return;

        try
        {
            var fileExists = await storageService.ExistsAsync(fileUrl);

            if (!fileExists) return;

            await storageService.DeleteAsync(fileUrl);
            logger.LogInformation("ReadingBookService:DeleteFileFromStorageAsync -> SUCCESSFULLY DELETED FILE FROM STORAGE: {FileUrl}", fileUrl);

        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "ReadingBookService:DeleteFileFromStorageAsync -> FAILED TO DELETE FILE FROM STORAGE FOR READING BOOK {Id}: {FileUrl}", readingBookId, fileUrl);
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
            
            logger.LogInformation("ReadingBookService:ExtractLeftSideColorAsync -> EXTRACTED COLOR: {Color}", hexColor);
            
            return hexColor;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "ReadingBookService:ExtractLeftSideColorAsync -> FAILED TO EXTRACT COLOR, USING DEFAULT");
            return "#CCCCCC"; // DEFAULT GRAY COLOR
        }
    }
    
    /// <summary>
    /// VERIFY OR CREATE READING IF IT DOESN'T EXIST
    /// </summary>
    private async Task<ServiceResult<Reading>> VerifyOrCreateReadingAsync(int readingId, string userId, int languageId)
    {
        var reading = await readingRepository.GetByIdAsync(readingId);

        if (reading is not null)
        {
            return ServiceResult<Reading>.Success(reading);
        }

        logger.LogWarning("ReadingBookService:VerifyOrCreateReadingAsync -> READING NOT FOUND WITH ID: {ReadingId}", readingId);

        var language = await languageRepository.GetByIdAsync(languageId);

        if (language is null)
        {
            logger.LogWarning("ReadingBookService:VerifyOrCreateReadingAsync -> LANGUAGE NOT FOUND FOR ID: {LanguageId}", languageId);
            return ServiceResult<Reading>.Fail("LANGUAGE NOT FOUND", HttpStatusCode.NotFound);
        }

        var practice = await practiceRepository.ExistsByLanguageIdAsync(languageId);

        if (practice is null)
        {
            logger.LogWarning("ReadingBookService:VerifyOrCreateReadingAsync -> PRACTICE NOT FOUND FOR LANGUAGE ID: {LanguageId}", languageId);
            return ServiceResult<Reading>.Fail("PRACTICE NOT FOUND FOR LANGUAGE", HttpStatusCode.NotFound);
        }

        reading = new Reading
        {
            UserId = userId,
            LanguageId = languageId,
            PracticeId = practice.Id,
            Language = language,
            Practice = practice
        };

        await readingRepository.CreateAsync(reading);

        logger.LogInformation("ReadingBookService:VerifyOrCreateReadingAsync -> NEW READING CREATED WITH ID: {ReadingId}", reading.Id);

        return ServiceResult<Reading>.Success(reading);
    }

    #endregion

    public async Task<ServiceResult<ReadingBookWithLanguageId>> GetReadingBookItemByIdAsync(int id)
    {

        logger.LogInformation("ReadingBookService:GetReadingBookItemByIdAsync --> FETCHING READING BOOK WITH ID: {Id}", id);

        var readingBook = await readingBookRepository.GetReadingBookItemByIdAsync(id);

        if (readingBook is null)
        {
            logger.LogWarning("ReadingBookService:GetReadingBookItemByIdAsync --> READING BOOK NOT FOUND WITH ID: {Id}", id);
            return ServiceResult<ReadingBookWithLanguageId>.Fail("READING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        logger.LogInformation("ReadingBookService:GetReadingBookItemByIdAsync --> SUCCESSFULLY FETCHED READING BOOK: {BookName}", readingBook.Name);

        var result = mapper.Map<ReadingBook, ReadingBookWithLanguageId>(readingBook);
        return ServiceResult<ReadingBookWithLanguageId>.Success(result);
    }

    public async Task<ServiceResult<PagedResult<ReadingBookWithTotalCount>>> GetAllRBooksWithPagingAsync(string userId, PagedRequest request)
    {

        if (string.IsNullOrWhiteSpace(userId))
        {
            logger.LogWarning("ReadingBookService:GetAllRBooksWithPagingAsync --> USER ID IS REQUIRED FOR FETCHING READING BOOKS");
            return ServiceResult<PagedResult<ReadingBookWithTotalCount>>.Fail("USER ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("ReadingBookService:GetAllRBooksWithPagingAsync --> FETCHING READING BOOKS FOR USER: {UserId}, PAGE: {Page}, PAGE SIZE: {PageSize}", userId, request.Page, request.PageSize);

        var (items, totalCount) = await readingBookRepository.GetAllRBooksWithPagingAsync(userId, request.Page, request.PageSize);

        logger.LogInformation("ReadingBookService:GetAllRBooksWithPagingAsync --> FETCHED {Count} READING BOOKS OUT OF {Total} FOR USER: {UserId}", items.Count, totalCount, userId);

        var mappedDtos = mapper.Map<List<ReadingBook>, List<ReadingBookDto>>(items);
        var mappedResult = new ReadingBookWithTotalCount
        {
            ReadingBookDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<ReadingBookWithTotalCount>.Create([mappedResult], request, totalCount);

        return ServiceResult<PagedResult<ReadingBookWithTotalCount>>.Success(result);
    }

    public async Task<ServiceResult<List<ReadingBookDto>>> GetRBookCreateItemsAsync(string userId, string language, string practice)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(userId))
        {
            logger.LogWarning("ReadingBookService:GetRBookCreateItemsAsync --> USER ID IS REQUIRED FOR FETCHING CREATE ITEMS");
            return ServiceResult<List<ReadingBookDto>>.Fail("USER ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("ReadingBookService:GetRBookCreateItemsAsync --> FETCHING READING BOOK CREATE ITEMS FOR USER: {UserId}", userId);

        // CHECK IF LANGUAGES EXIST
        var languageExists = await languageRepository.ExistsByNameAsync(language);

        if (languageExists is null)
        {
            logger.LogWarning("ReadingBookService:GetRBookCreateItemsAsync --> LANGUAGE NOT FOUND: {Language}", language);
            return ServiceResult<List<ReadingBookDto>>.Fail($"LANGUAGE '{language}' NOT FOUND",
                HttpStatusCode.NotFound);
        }

        // CHECK IF PRACTICE EXIST
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(practice, languageExists.Id);

        if (practiceExists is null)
        {
            logger.LogWarning("ReadingBookService:GetRBookCreateItemsAsync --> PRACTICE NOT FOUND: {Practice} FOR LANGUAGE: {Language}", practice, language);
            return ServiceResult<List<ReadingBookDto>>.Fail($"PRACTICE '{practice}' NOT FOUND FOR LANGUAGE '{language}'.",
                HttpStatusCode.NotFound);
        }

        var readingBooks = await readingBookRepository.GetRBookCreateItemsAsync(userId, languageExists.Id, practiceExists.Id);

        var result = mapper.Map<List<ReadingBook>, List<ReadingBookDto>>(readingBooks);

        logger.LogInformation("ReadingBookService:GetRBookCreateItemsAsync -> SUCCESSFULLY FETCHED {Count} CREATE ITEMS", result.Count);

        return ServiceResult<List<ReadingBookDto>>.Success(result);
    }

    public async Task<ServiceResult> DeleteRBookItemByIdAsync(int id)
    {
        // GUARD CLAUSE
        if (id <= 0)
        {
            logger.LogWarning("ReadingBookService:DeleteRBookItemByIdAsync -> INVALID READING BOOK ID FOR DELETION: {Id}", id);
            return ServiceResult.Fail("INVALID READING BOOK ID", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("ReadingBookService:DeleteRBookItemByIdAsync -> ATTEMPTING TO DELETE READING BOOK WITH ID: {Id}", id);

        var readingBook = await readingBookRepository.GetByIdAsync(id);

        // FAST FAIL
        if (readingBook is null)
        {
            logger.LogWarning("ReadingBookService:DeleteRBookItemByIdAsync -> READING BOOK NOT FOUND FOR DELETION WITH ID: {Id}", id);
            return ServiceResult.Fail("READING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        // STORE FILE PATHS BEFORE DELETION
        var imageUrl = readingBook.ImageUrl;
        var sourceUrl = readingBook.SourceUrl;

        readingBookRepository.Delete(readingBook);
        await unitOfWork.CommitAsync();

        logger.LogInformation("ReadingBookService:DeleteRBookItemByIdAsync -> SUCCESSFULLY DELETED READING BOOK FROM DATABASE WITH ID: {Id}", id);

        // DELETE FILES FROM STORAGE AFTER DATABASE DELETION
        await DeleteFileFromStorageAsync(id, imageUrl);
        await DeleteFileFromStorageAsync(id, sourceUrl);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult<ReadingBookDto>> ReadingBookAddAsync(CreateReadingBookRequest request)
    {
        logger.LogInformation("ReadingBookService:ReadingBookAddAsync -> CREATING NEW READING BOOK FOR READING ID: {ReadingId}", request.ReadingId);

        // VERIFY OR CREATE READING
        var readingResult = await VerifyOrCreateReadingAsync(request.ReadingId, request.UserId, request.LanguageId);

        // FAST FAIL
        if (!readingResult.IsSuccess)
        {
            return ServiceResult<ReadingBookDto>.Fail(readingResult.ErrorMessage!, readingResult.Status);
        }

        var reading = readingResult.Data!;

        try
        {
            // UPLOAD FILES TO STORAGE
            logger.LogInformation("ReadingBookService:ReadingBookAddAsync -> UPLOADING FILES TO STORAGE");

            var imageUrl = await UploadFileToStorageAsync(request.ImageFile, request.UserId, "rbooks");
            var sourceUrl = await UploadFileToStorageAsync(request.SourceFile, request.UserId, "rbooks");

            // EXTRACT LEFT SIDE COLOR FROM IMAGE
            logger.LogInformation("ReadingBookService:ReadingBookAddAsync -> EXTRACTING LEFT SIDE COLOR");
            var leftColor = await ExtractLeftSideColorAsync(request.ImageFile);

            // CREATE READING BOOK
            var readingBook = new ReadingBook
            {
                ReadingId = reading.Id,
                Name = request.Name,
                ImageUrl = imageUrl,
                LeftColor = leftColor,
                SourceUrl = sourceUrl,
                Reading = reading
            };

            await readingBookRepository.CreateAsync(readingBook);
            await unitOfWork.CommitAsync();

            logger.LogInformation("ReadingBookService:ReadingBookAddAsync -> SUCCESSFULLY CREATED READING BOOK WITH ID: {Id}, NAME: {Name}", readingBook.Id, readingBook.Name);

            var result = mapper.Map<ReadingBook, ReadingBookDto>(readingBook);
            return ServiceResult<ReadingBookDto>.SuccessAsCreated(result, $"/api/ReadingBook/{readingBook.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ReadingBookService:ReadingBookAddAsync -> ERROR CREATING READING BOOK");
            return ServiceResult<ReadingBookDto>.Fail("ERROR CREATING READING BOOK", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ServiceResult<ReadingBookDto>> ReadingBookUpdateAsync(UpdateReadingBookRequest request)
    {
        logger.LogInformation("ReadingBookService:ReadingBookUpdateAsync -> UPDATING READING BOOK WITH ID: {Id}", request.Id);

        // VERIFY READING BOOK EXISTS
        var existingBook = await readingBookRepository.GetByIdAsync(request.Id);

        // FAST FAIL
        if (existingBook is null)
        {
            logger.LogWarning("ReadingBookService:ReadingBookUpdateAsync -> READING BOOK NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<ReadingBookDto>.Fail("READING BOOK NOT FOUND", HttpStatusCode.NotFound);
        }

        // VERIFY OR CREATE READING
        var readingResult = await VerifyOrCreateReadingAsync(request.ReadingId, request.UserId, request.LanguageId);

        // FAST FAIL
        if (!readingResult.IsSuccess)
        {
            return ServiceResult<ReadingBookDto>.Fail(readingResult.ErrorMessage!, readingResult.Status);
        }

        var reading = readingResult.Data!;

        try
        {
            // STORE OLD FILE URLS FOR DELETION
            var oldImageUrl = existingBook.ImageUrl;
            var oldSourceUrl = existingBook.SourceUrl;

            // UPDATE IMAGE IF NEW FILE PROVIDED
            if (request.ImageFile is not null)
            {
                logger.LogInformation("ReadingBookService:ReadingBookUpdateAsync -> UPLOADING NEW IMAGE FILE");
                existingBook.ImageUrl = await UploadFileToStorageAsync(request.ImageFile, request.UserId, "rbooks");
                existingBook.LeftColor = await ExtractLeftSideColorAsync(request.ImageFile);
            }

            // UPDATE SOURCE IF NEW FILE PROVIDED
            if (request.SourceFile is not null)
            {
                logger.LogInformation("ReadingBookService:ReadingBookUpdateAsync -> UPLOADING NEW SOURCE FILE");
                existingBook.SourceUrl = await UploadFileToStorageAsync(request.SourceFile, request.UserId, "rbooks");
            }

            // UPDATE OTHER FIELDS
            existingBook.ReadingId = reading.Id;
            existingBook.Name = request.Name;

            readingBookRepository.Update(existingBook);
            await unitOfWork.CommitAsync();

            logger.LogInformation("ReadingBookService:ReadingBookUpdateAsync -> SUCCESSFULLY UPDATED READING BOOK WITH ID: {Id}", existingBook.Id);

            // DELETE OLD FILES FROM STORAGE IF URLS CHANGED
            if (request.ImageFile is not null && !string.Equals(oldImageUrl, existingBook.ImageUrl, StringComparison.OrdinalIgnoreCase))
            {
                await DeleteFileFromStorageAsync(request.Id, oldImageUrl);
            }

            if (request.SourceFile is not null && !string.Equals(oldSourceUrl, existingBook.SourceUrl, StringComparison.OrdinalIgnoreCase))
            {
                await DeleteFileFromStorageAsync(request.Id, oldSourceUrl);
            }

            var result = mapper.Map<ReadingBook, ReadingBookDto>(existingBook);
            return ServiceResult<ReadingBookDto>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ReadingBookService:ReadingBookUpdateAsync -> ERROR UPDATING READING BOOK");
            return ServiceResult<ReadingBookDto>.Fail("ERROR UPDATING READING BOOK", HttpStatusCode.InternalServerError);
        }
    }
}
