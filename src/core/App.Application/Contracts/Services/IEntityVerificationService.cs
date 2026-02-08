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
    Task<ServiceResult<Flashcard>> VerifyOrCreateFlashcardAsync(int practiceId, string userId, int languageId);

    /// <summary>
    /// VERIFIES IF A READING EXISTS, OR CREATES A NEW ONE IF IT DOESN'T.
    /// </summary>
    Task<ServiceResult<Reading>> VerifyOrCreateReadingAsync(int practiceId, string userId, int languageId);

    /// <summary>
    /// VERIFIES IF A WRITING EXISTS, OR CREATES A NEW ONE IF IT DOESN'T.
    /// </summary>
    Task<ServiceResult<Writing>> VerifyOrCreateWritingAsync(int practiceId, string userId, int languageId);
}
