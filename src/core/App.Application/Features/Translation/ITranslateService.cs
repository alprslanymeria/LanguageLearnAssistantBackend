using App.Application.Common;
using App.Application.Features.Translation.Dtos;

namespace App.Application.Features.Translation;

/// <summary>
/// SERVICE INTERFACE FOR TEXT TRANSLATION OPERATIONS.
/// </summary>
public interface ITranslateService
{
    /// <summary>
    /// TRANSLATES TEXT BASED ON PRACTICE TYPE AND USER PREFERENCES.
    /// </summary>
    Task<ServiceResult<TranslateTextResponse>> TranslateTextAsync(

        TranslateTextRequest request,
        string userId,
        string accessToken,
        CancellationToken cancellationToken = default);
}
