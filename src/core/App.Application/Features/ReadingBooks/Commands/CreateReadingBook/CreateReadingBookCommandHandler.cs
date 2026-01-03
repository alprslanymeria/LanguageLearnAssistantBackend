using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Files;
using App.Application.Contracts.Infrastructure.Storage;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingBooks.Dtos;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Net;

namespace App.Application.Features.ReadingBooks.Commands.CreateReadingBook;

/// <summary>
/// HANDLER FOR CREATE READING BOOK COMMAND.
/// </summary>
public class CreateReadingBookCommandHandler(
    IReadingBookRepository readingBookRepository,
    IReadingRepository readingRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService,
    IMapper mapper,
    ILogger<CreateReadingBookCommandHandler> logger
    ) : ICommandHandler<CreateReadingBookCommand, ServiceResult<ReadingBookDto>>
{
    public async Task<ServiceResult<ReadingBookDto>> Handle(
        CreateReadingBookCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateReadingBookCommandHandler -> CREATING NEW READING BOOK FOR READING ID: {ReadingId}", request.ReadingId);

        // VERIFY OR CREATE READING
        var readingResult = await VerifyOrCreateReadingAsync(request.ReadingId, request.UserId, request.LanguageId);

        if (!readingResult.IsSuccess)
        {
            return ServiceResult<ReadingBookDto>.Fail(readingResult.ErrorMessage!, readingResult.Status);
        }

        var reading = readingResult.Data!;

        try
        {
            // UPLOAD FILES TO STORAGE
            logger.LogInformation("CreateReadingBookCommandHandler -> UPLOADING FILES TO STORAGE");

            var imageUrl = await UploadFileToStorageAsync(request.ImageFile, request.UserId, "rbooks");
            var sourceUrl = await UploadFileToStorageAsync(request.SourceFile, request.UserId, "rbooks");

            // EXTRACT LEFT SIDE COLOR FROM IMAGE
            logger.LogInformation("CreateReadingBookCommandHandler -> EXTRACTING LEFT SIDE COLOR");
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

            logger.LogInformation("CreateReadingBookCommandHandler -> SUCCESSFULLY CREATED READING BOOK WITH ID: {Id}, NAME: {Name}", 
                readingBook.Id, readingBook.Name);

            var result = mapper.Map<ReadingBook, ReadingBookDto>(readingBook);
            return ServiceResult<ReadingBookDto>.SuccessAsCreated(result, $"/api/ReadingBook/{readingBook.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateReadingBookCommandHandler -> ERROR CREATING READING BOOK");
            return ServiceResult<ReadingBookDto>.Fail("ERROR CREATING READING BOOK", HttpStatusCode.InternalServerError);
        }
    }

    #region PRIVATE METHODS

    private async Task<string> UploadFileToStorageAsync(IFileUpload file, string userId, string folderName)
    {
        var fileName = $"{userId}/{folderName}/{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}_{file.FileName}";
        
        using var stream = file.OpenReadStream();
        var fileUrl = await storageService.UploadAsync(stream, fileName, file.ContentType, null);
        
        logger.LogInformation("CreateReadingBookCommandHandler -> SUCCESSFULLY UPLOADED FILE TO STORAGE: {FileUrl}", fileUrl);
        
        return fileUrl;
    }

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
            
            logger.LogInformation("CreateReadingBookCommandHandler -> EXTRACTED COLOR: {Color}", hexColor);
            
            return hexColor;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "CreateReadingBookCommandHandler -> FAILED TO EXTRACT COLOR, USING DEFAULT");
            return "#CCCCCC"; // DEFAULT GRAY COLOR
        }
    }

    private async Task<ServiceResult<Reading>> VerifyOrCreateReadingAsync(int readingId, string userId, int languageId)
    {
        var reading = await readingRepository.GetByIdAsync(readingId);

        if (reading is not null)
        {
            return ServiceResult<Reading>.Success(reading);
        }

        logger.LogWarning("CreateReadingBookCommandHandler -> READING NOT FOUND WITH ID: {ReadingId}", readingId);

        var language = await languageRepository.GetByIdAsync(languageId);

        if (language is null)
        {
            logger.LogWarning("CreateReadingBookCommandHandler -> LANGUAGE NOT FOUND FOR ID: {LanguageId}", languageId);
            return ServiceResult<Reading>.Fail("LANGUAGE NOT FOUND", HttpStatusCode.NotFound);
        }

        var practice = await practiceRepository.ExistsByLanguageIdAsync(languageId);

        if (practice is null)
        {
            logger.LogWarning("CreateReadingBookCommandHandler -> PRACTICE NOT FOUND FOR LANGUAGE ID: {LanguageId}", languageId);
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

        logger.LogInformation("CreateReadingBookCommandHandler -> NEW READING CREATED WITH ID: {ReadingId}", reading.Id);

        return ServiceResult<Reading>.Success(reading);
    }

    #endregion
}
