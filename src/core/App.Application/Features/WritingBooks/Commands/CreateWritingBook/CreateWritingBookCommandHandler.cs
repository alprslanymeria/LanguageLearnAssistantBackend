using App.Application.Common;
using App.Application.Common.CQRS;
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

namespace App.Application.Features.WritingBooks.Commands.CreateWritingBook;

/// <summary>
/// HANDLER FOR CREATE WRITING BOOK COMMAND.
/// </summary>
public class CreateWritingBookCommandHandler(
    IWritingBookRepository writingBookRepository,
    IWritingRepository writingRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService,
    IMapper mapper,
    ILogger<CreateWritingBookCommandHandler> logger
    ) : ICommandHandler<CreateWritingBookCommand, ServiceResult<WritingBookDto>>
{
    public async Task<ServiceResult<WritingBookDto>> Handle(
        CreateWritingBookCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateWritingBookCommandHandler -> CREATING NEW WRITING BOOK FOR WRITING ID: {WritingId}", request.WritingId);

        // VERIFY OR CREATE WRITING
        var writingResult = await VerifyOrCreateWritingAsync(request.WritingId, request.UserId, request.LanguageId);

        if (!writingResult.IsSuccess)
        {
            return ServiceResult<WritingBookDto>.Fail(writingResult.ErrorMessage!, writingResult.Status);
        }

        var writing = writingResult.Data!;

        try
        {
            // UPLOAD FILES TO STORAGE
            logger.LogInformation("CreateWritingBookCommandHandler -> UPLOADING FILES TO STORAGE");

            var imageUrl = await UploadFileToStorageAsync(request.ImageFile, request.UserId, "wbooks");
            var sourceUrl = await UploadFileToStorageAsync(request.SourceFile, request.UserId, "wbooks");

            // EXTRACT LEFT SIDE COLOR FROM IMAGE
            logger.LogInformation("CreateWritingBookCommandHandler -> EXTRACTING LEFT SIDE COLOR");
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

            logger.LogInformation("CreateWritingBookCommandHandler -> SUCCESSFULLY CREATED WRITING BOOK WITH ID: {Id}, NAME: {Name}", 
                writingBook.Id, writingBook.Name);

            var result = mapper.Map<WritingBook, WritingBookDto>(writingBook);
            return ServiceResult<WritingBookDto>.SuccessAsCreated(result, $"/api/WritingBook/{writingBook.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateWritingBookCommandHandler -> ERROR CREATING WRITING BOOK");
            return ServiceResult<WritingBookDto>.Fail("ERROR CREATING WRITING BOOK", HttpStatusCode.InternalServerError);
        }
    }

    #region PRIVATE METHODS

    private async Task<string> UploadFileToStorageAsync(IFileUpload file, string userId, string folderName)
    {
        var fileName = $"{userId}/{folderName}/{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}_{file.FileName}";
        
        using var stream = file.OpenReadStream();
        var fileUrl = await storageService.UploadAsync(stream, fileName, file.ContentType, null);
        
        logger.LogInformation("CreateWritingBookCommandHandler -> SUCCESSFULLY UPLOADED FILE TO STORAGE: {FileUrl}", fileUrl);
        
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
            
            logger.LogInformation("CreateWritingBookCommandHandler -> EXTRACTED COLOR: {Color}", hexColor);
            
            return hexColor;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "CreateWritingBookCommandHandler -> FAILED TO EXTRACT COLOR, USING DEFAULT");
            return "#CCCCCC"; // DEFAULT GRAY COLOR
        }
    }

    private async Task<ServiceResult<Writing>> VerifyOrCreateWritingAsync(int writingId, string userId, int languageId)
    {
        var writing = await writingRepository.GetByIdAsync(writingId);

        if (writing is not null)
        {
            return ServiceResult<Writing>.Success(writing);
        }

        logger.LogWarning("CreateWritingBookCommandHandler -> WRITING NOT FOUND WITH ID: {WritingId}", writingId);

        var language = await languageRepository.GetByIdAsync(languageId);

        if (language is null)
        {
            logger.LogWarning("CreateWritingBookCommandHandler -> LANGUAGE NOT FOUND FOR ID: {LanguageId}", languageId);
            return ServiceResult<Writing>.Fail("LANGUAGE NOT FOUND", HttpStatusCode.NotFound);
        }

        var practice = await practiceRepository.ExistsByLanguageIdAsync(languageId);

        if (practice is null)
        {
            logger.LogWarning("CreateWritingBookCommandHandler -> PRACTICE NOT FOUND FOR LANGUAGE ID: {LanguageId}", languageId);
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

        logger.LogInformation("CreateWritingBookCommandHandler -> NEW WRITING CREATED WITH ID: {WritingId}", writing.Id);

        return ServiceResult<Writing>.Success(writing);
    }

    #endregion
}
