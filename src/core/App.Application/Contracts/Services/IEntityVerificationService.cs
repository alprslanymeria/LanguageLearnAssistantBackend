using App.Application.Common;
using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.ReadingEntities;
using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Services;

/// <summary>
/// SERVICE FOR VERIFYING OR CREATING ENTITIES WHEN THEY DON'T EXIST.
/// THIS PROVIDES A CENTRALIZED LOGIC FOR ENTITY VERIFICATION ACROSS HANDLERS.
/// </summary>
public interface IEntityVerificationService
{
    /// <summary>
    /// VERIFIES IF A FLASHCARD EXISTS, OR CREATES A NEW ONE IF IT DOESN'T.
    /// </summary>
    /// <param name="flashcardId">THE ID OF THE FLASHCARD TO VERIFY</param>
    /// <param name="userId">THE USER ID FOR CREATING A NEW FLASHCARD</param>
    /// <param name="languageId">THE LANGUAGE ID FOR CREATING A NEW FLASHCARD</param>
    /// <returns>SERVICE RESULT CONTAINING THE EXISTING OR NEWLY CREATED FLASHCARD</returns>
    Task<ServiceResult<Flashcard>> VerifyOrCreateFlashcardAsync(int flashcardId, string userId, int languageId);

    /// <summary>
    /// VERIFIES IF A READING EXISTS, OR CREATES A NEW ONE IF IT DOESN'T.
    /// </summary>
    /// <param name="readingId">THE ID OF THE READING TO VERIFY</param>
    /// <param name="userId">THE USER ID FOR CREATING A NEW READING</param>
    /// <param name="languageId">THE LANGUAGE ID FOR CREATING A NEW READING</param>
    /// <returns>SERVICE RESULT CONTAINING THE EXISTING OR NEWLY CREATED READING</returns>
    Task<ServiceResult<Reading>> VerifyOrCreateReadingAsync(int readingId, string userId, int languageId);

    /// <summary>
    /// VERIFIES IF A WRITING EXISTS, OR CREATES A NEW ONE IF IT DOESN'T.
    /// </summary>
    /// <param name="writingId">THE ID OF THE WRITING TO VERIFY</param>
    /// <param name="userId">THE USER ID FOR CREATING A NEW WRITING</param>
    /// <param name="languageId">THE LANGUAGE ID FOR CREATING A NEW WRITING</param>
    /// <returns>SERVICE RESULT CONTAINING THE EXISTING OR NEWLY CREATED WRITING</returns>
    Task<ServiceResult<Writing>> VerifyOrCreateWritingAsync(int writingId, string userId, int languageId);
}
