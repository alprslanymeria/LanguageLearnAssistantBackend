using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.ExternalApi;
using App.Application.Contracts.Infrastructure.Translation;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Translation.Dtos;
using App.Domain.Exceptions;

namespace App.Application.Features.Translation.Queries.TranslateText;

public class TranslateTextQueryHandler(

    ITranslationProvider translationProvider,
    IUserApiClient userApiClient,
    ILanguageRepository languageRepository

    ) : IQueryHandler<TranslateTextQuery, ServiceResult<TranslateTextResponse>>
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
        var userInfo = await userApiClient.GetProfileInfos(userId, accessToken, cancellationToken);

        if (userInfo is null)
        {
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
            return null;
        }

        return LanguageIdToCodeMap.GetValueOrDefault(language.Id);
    }

    #endregion


    public async Task<ServiceResult<TranslateTextResponse>> Handle(

        TranslateTextQuery request,
        CancellationToken cancellationToken)
    {

        // VALIDATE PRACTICE TYPE
        var practiceType = request.Request.Practice.ToLowerInvariant();

        if (!ValidPracticeTypes.Contains(practiceType))
        {
            throw new BusinessException("INVALID PRACTICE TYPE. VALID VALUES: READING, LISTENING, WRITING");
        }

        // DETERMINE TARGET LANGUAGE CODE
        var targetLanguageCode = await DetermineTargetLanguageAsync(practiceType, request.Request.Language, request.UserId, request.AccessToken, cancellationToken);

        if (string.IsNullOrEmpty(targetLanguageCode))
        {
            throw new BusinessException("TARGET LANGUAGE COULD NOT BE DETERMINED");
        }

        // PERFORM TRANSLATION
        var translatedText = await translationProvider.TranslateAsync(request.Request.SelectedText, targetLanguageCode, cancellationToken);

        var response = new TranslateTextResponse(

            OriginalText: request.Request.SelectedText,
            TranslatedText: translatedText,
            TargetLanguageCode: targetLanguageCode
            );

        return ServiceResult<TranslateTextResponse>.Success(response);
    }
}
