using System.Net;
using App.Application.Common;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Contracts.Services;
using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.ReadingEntities;
using App.Domain.Entities.WritingEntities;
using Microsoft.Extensions.Logging;

namespace App.Integration;

/// <summary>
/// SERVICE FOR VERIFYING OR CREATING ENTITIES WHEN THEY DON'T EXIST.
/// </summary>
public class EntityVerificationService(

    IFlashcardRepository flashcardRepository,
    IReadingRepository readingRepository,
    IWritingRepository writingRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IUnitOfWork unitOfWork,
    ILogger<EntityVerificationService> logger

    ) : IEntityVerificationService
{
    public async Task<ServiceResult<Flashcard>> VerifyOrCreateFlashcardAsync(int practiceId, string userId, int languageId)
    {
        var flashcard = await flashcardRepository.GetByPracticeIdUserIdLanguageIdAsync(practiceId, userId, languageId);

        if (flashcard is not null)
        {
            return ServiceResult<Flashcard>.Success(flashcard);
        }

        logger.LogWarning("EntityVerificationService -> FLASHCARD NOT FOUND");

        var language = await languageRepository.GetByIdAsync(languageId);

        if (language is null)
        {
            logger.LogWarning("EntityVerificationService -> LANGUAGE NOT FOUND FOR ID: {LanguageId}", languageId);
            return ServiceResult<Flashcard>.Fail("LANGUAGE NOT FOUND", HttpStatusCode.NotFound);
        }

        var practice = await practiceRepository.ExistsByNameAndLanguageIdAsync("flashcard", languageId);

        if (practice is null)
        {
            logger.LogWarning("EntityVerificationService -> PRACTICE NOT FOUND FOR LANGUAGE ID: {LanguageId}", languageId);
            return ServiceResult<Flashcard>.Fail("PRACTICE NOT FOUND FOR LANGUAGE", HttpStatusCode.NotFound);
        }

        flashcard = new Flashcard
        {
            UserId = userId,
            LanguageId = languageId,
            PracticeId = practice.Id
        };

        await flashcardRepository.AddAsync(flashcard);
        await unitOfWork.CommitAsync();

        logger.LogInformation("EntityVerificationService -> NEW FLASHCARD CREATED WITH ID: {FlashcardId}", flashcard.Id);

        return ServiceResult<Flashcard>.Success(flashcard);
    }

    public async Task<ServiceResult<Reading>> VerifyOrCreateReadingAsync(int practiceId, string userId, int languageId)
    {
        var reading = await readingRepository.GetByPracticeIdUserIdLanguageIdAsync(practiceId, userId, languageId);

        if (reading is not null)
        {
            return ServiceResult<Reading>.Success(reading);
        }

        logger.LogWarning("EntityVerificationService -> READING NOT FOUND");

        var language = await languageRepository.GetByIdAsync(languageId);

        if (language is null)
        {
            logger.LogWarning("EntityVerificationService -> LANGUAGE NOT FOUND FOR ID: {LanguageId}", languageId);
            return ServiceResult<Reading>.Fail("LANGUAGE NOT FOUND", HttpStatusCode.NotFound);
        }

        var practice = await practiceRepository.ExistsByNameAndLanguageIdAsync("reading", languageId);

        if (practice is null)
        {
            logger.LogWarning("EntityVerificationService -> PRACTICE NOT FOUND FOR LANGUAGE ID: {LanguageId}", languageId);
            return ServiceResult<Reading>.Fail("PRACTICE NOT FOUND FOR LANGUAGE", HttpStatusCode.NotFound);
        }

        reading = new Reading
        {
            UserId = userId,
            LanguageId = languageId,
            PracticeId = practice.Id
        };

        await readingRepository.AddAsync(reading);
        await unitOfWork.CommitAsync();

        logger.LogInformation("EntityVerificationService -> NEW READING CREATED WITH ID: {ReadingId}", reading.Id);

        return ServiceResult<Reading>.Success(reading);
    }

    public async Task<ServiceResult<Writing>> VerifyOrCreateWritingAsync(int practiceId, string userId, int languageId)
    {
        var writing = await writingRepository.GetByPracticeIdUserIdLanguageIdAsync(practiceId, userId, languageId);

        if (writing is not null)
        {
            return ServiceResult<Writing>.Success(writing);
        }

        logger.LogWarning("EntityVerificationService -> WRITING NOT FOUND");

        var language = await languageRepository.GetByIdAsync(languageId);

        if (language is null)
        {
            logger.LogWarning("EntityVerificationService -> LANGUAGE NOT FOUND FOR ID: {LanguageId}", languageId);
            return ServiceResult<Writing>.Fail("LANGUAGE NOT FOUND", HttpStatusCode.NotFound);
        }

        var practice = await practiceRepository.ExistsByNameAndLanguageIdAsync("writing", languageId);

        if (practice is null)
        {
            logger.LogWarning("EntityVerificationService -> PRACTICE NOT FOUND FOR LANGUAGE ID: {LanguageId}", languageId);
            return ServiceResult<Writing>.Fail("PRACTICE NOT FOUND FOR LANGUAGE", HttpStatusCode.NotFound);
        }

        writing = new Writing
        {
            UserId = userId,
            LanguageId = languageId,
            PracticeId = practice.Id
        };

        await writingRepository.AddAsync(writing);
        await unitOfWork.CommitAsync();

        logger.LogInformation("EntityVerificationService -> NEW WRITING CREATED WITH ID: {WritingId}", writing.Id);

        return ServiceResult<Writing>.Success(writing);
    }
}
