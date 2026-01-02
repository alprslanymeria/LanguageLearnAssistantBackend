using App.Application.Common;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Languages.Dtos;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.Languages;

/// <summary>
/// SERVICE IMPLEMENTATION FOR LANGUAGE OPERATIONS.
/// </summary>
public class LanguageService(

    ILanguageRepository languageRepository,
    IMapper mapper,
    ILogger<LanguageService> logger
    
    ) : ILanguageService
{
    public async Task<ServiceResult<List<LanguageDto>>> GetLanguagesAsync()
    {
        logger.LogInformation("LanguageService:GetLanguagesAsync -> FETCHING ALL LANGUAGES");

        var languages = await languageRepository.GetLanguagesAsync();

        if(languages.Count == 0)
        {
            logger.LogInformation("LanguageService:GetLanguagesAsync -> LANGUAGES IS EMPTY");
            return ServiceResult<List<LanguageDto>>.Success([]);
        }

        logger.LogInformation("LanguageService:GetLanguagesAsync -> SUCCESSFULLY FETCHED {Count} LANGUAGES", languages.Count);

        var result = mapper.Map<List<LanguageDto>>(languages);

        return ServiceResult<List<LanguageDto>>.Success(result);
    }
}
