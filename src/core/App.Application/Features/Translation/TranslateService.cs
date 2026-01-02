using App.Application.Common;
using App.Application.Contracts.Infrastructure.ExternalApi;
using App.Application.Contracts.Infrastructure.Translation;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Translation.Dtos;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.Translation;

/// <summary>
/// SERVICE IMPLEMENTATION FOR TEXT TRANSLATION OPERATIONS.
/// </summary>
public class TranslateService(

    ITranslationProvider translationProvider,
    IUserApiClient userApiClient,
    ILanguageRepository languageRepository,
    ILogger<TranslateService> logger

    ) : ITranslateService

{
    private static readonly Dictionary<int, string> LanguageIdToCodeMap = new()
    {
        { 1, "en" },
        { 2, "tr" },
        { 3, "de" },
        { 4, "ru" }
    };

    private static readonly HashSet<string> ValidPracticeTypes = ["reading", "listening", "writing"];

    #region UTILS

    /// <summary>
    /// DETERMINES THE TARGET LANGUAGE CODE BASED ON PRACTICE TYPE AND USER PREFERENCES.
    /// </summary>
    private async Task<string?> DetermineTargetLanguageAsync(string practiceType, string? languageName, string userId, string accessToken, CancellationToken cancellationToken)
    {
        if (practiceType is "reading" or "listening")
        {
            // GET USER'S NATIVE LANGUAGE FROM EXTERNAL OAUTH SERVER
            return await GetUserNativeLanguageCodeAsync(userId, accessToken, cancellationToken);
        }

        if (practiceType == "writing" && !string.IsNullOrEmpty(languageName))
        {
            // GET TARGET LANGUAGE FROM DATABASE
            return await GetLanguageCodeByNameAsync(languageName);
        }

        return null;
    }

    /// <summary>
    /// GETS THE USER'S NATIVE LANGUAGE CODE FROM THE EXTERNAL OAUTH SERVER.
    /// </summary>

    private async Task<string?> GetUserNativeLanguageCodeAsync(string userId, string accessToken, CancellationToken cancellationToken)
    {
        var userInfo = await userApiClient.GetUserInfoAsync(userId, accessToken, cancellationToken);

        if (userInfo is null)
        {
            logger.LogWarning("TranslateService:GetUserNativeLanguageCodeAsync -> USER NOT FOUND: {UserId}", userId);
            return null;
        }

        return LanguageIdToCodeMap.GetValueOrDefault(userInfo.NativeLanguageId);
    }

    /// <summary>
    /// GETS THE LANGUAGE CODE BASED ON THE LANGUAGE NAME.
    /// </summary>
    private async Task<string?> GetLanguageCodeByNameAsync(string languageName)
    {
        var language = await languageRepository.ExistsByNameAsync(languageName);

        if (language is null)
        {
            logger.LogWarning("TranslateService:GetLanguageCodeByNameAsync -> LANGUAGE NOT FOUND: {LanguageName}", languageName);
            return null;
        }

        return LanguageIdToCodeMap.GetValueOrDefault(language.Id);
    }

    #endregion

    public async Task<ServiceResult<TranslateTextResponse>> TranslateTextAsync(

        TranslateTextRequest request,
        string userId,
        string accessToken,
        CancellationToken cancellationToken = default)
    {

        logger.LogInformation("TranslateService:TranslateTextAsync -> STARTING TRANSLATION FOR USER {UserId}, PRACTICE: {Practice}", userId, request.Practice);

        // VALIDATE PRACTICE TYPE
        var practiceType = request.Practice.ToLowerInvariant();

        if (!ValidPracticeTypes.Contains(practiceType))
        {
            logger.LogWarning("TranslateService:TranslateTextAsync -> INVALID PRACTICE TYPE: {Practice}", request.Practice);
            return ServiceResult<TranslateTextResponse>.Fail("INVALID PRACTICE TYPE. VALID VALUES: READING, LISTENING, WRITING", HttpStatusCode.BadRequest);
        }

        // DETERMINE TARGET LANGUAGE CODE
        var targetLanguageCode = await DetermineTargetLanguageAsync(practiceType, request.Language, userId, accessToken, cancellationToken);

        if (string.IsNullOrEmpty(targetLanguageCode))
        {
            logger.LogWarning("TranslateService:TranslateTextAsync -> COULD NOT DETERMINE TARGET LANGUAGE");
            return ServiceResult<TranslateTextResponse>.Fail("TARGET LANGUAGE COULD NOT BE DETERMINED", HttpStatusCode.BadRequest);
        }

        // PERFORM TRANSLATION
        try
        {
            var translatedText = await translationProvider.TranslateAsync(request.SelectedText, targetLanguageCode, cancellationToken);

            logger.LogInformation("TranslateService:TranslateTextAsync -> TEXT SUCCESSFULLY TRANSLATED TO {TargetLanguage}",targetLanguageCode);

            var response = new TranslateTextResponse
            {
                OriginalText = request.SelectedText,
                TranslatedText = translatedText,
                TargetLanguage = targetLanguageCode
            };

            return ServiceResult<TranslateTextResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "TranslateService:TranslateTextAsync -> TRANSLATION FAILED");
            return ServiceResult<TranslateTextResponse>.Fail("TRANSLATION SERVICE ERROR", HttpStatusCode.InternalServerError);
        }
    }
}
