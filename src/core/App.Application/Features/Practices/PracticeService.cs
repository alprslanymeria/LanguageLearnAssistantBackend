using App.Application.Common;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Practices.Dtos;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.Practices;

/// <summary>
/// SERVICE IMPLEMENTATION FOR PRACTICE OPERATIONS.
/// </summary>
public class PracticeService(

    IPracticeRepository practiceRepository,
    ILanguageRepository languageRepository,
    IMapper mapper,
    ILogger<PracticeService> logger
    
    ) : IPracticeService
{
    public async Task<ServiceResult<List<PracticeDto>>> GetPracticesByLanguageAsync(string language)
    {
        logger.LogInformation("PracticeService:GetPracticesByLanguageAsync -> FETCHING PRACTICES FOR LANGUAGE: {Language}", language);

        // CHECK IF LANGUAGES EXIST
        var languageExists = await languageRepository.ExistsByNameAsync(language);

        if (languageExists is null)
        {
            logger.LogWarning("PracticeService:GetPracticesByLanguageAsync -> LANGUAGE NOT FOUND: {Language}", language);
            return ServiceResult<List<PracticeDto>>.Fail($"LANGUAGE '{language}' NOT FOUND.",
                HttpStatusCode.NotFound);
        }

        var practices = await practiceRepository.GetPracticesByLanguageAsync(language);

        logger.LogInformation("PracticeService:GetPracticesByLanguageAsync -> SUCCESSFULLY FETCHED {Count} PRACTICES FOR LANGUAGE: {Language}", practices.Count, language);

        var result = mapper.Map<List<PracticeDto>>(practices);

        return ServiceResult<List<PracticeDto>>.Success(result);
    }
}
