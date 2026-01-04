using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Practices.Dtos;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.Practices.Queries.GetPracticesByLanguage;

/// <summary>
/// HANDLER FOR GET PRACTICES BY LANGUAGE QUERY.
/// </summary>
public class GetPracticesByLanguageQueryHandler(

    IPracticeRepository practiceRepository,
    ILanguageRepository languageRepository,
    IMapper mapper,
    ILogger<GetPracticesByLanguageQueryHandler> logger

    ) : IQueryHandler<GetPracticesByLanguageQuery, ServiceResult<List<PracticeDto>>>
{
    public async Task<ServiceResult<List<PracticeDto>>> Handle(

        GetPracticesByLanguageQuery request, 
        CancellationToken cancellationToken)
    {

        var language = request.Language.Trim().ToLower();

        logger.LogInformation("GetPracticesByLanguageQueryHandler -> FETCHING PRACTICES FOR LANGUAGE: {Language}", language);

        // CHECK IF LANGUAGES EXIST
        var languageExists = await languageRepository.ExistsByNameAsync(language);

        if (languageExists is null)
        {
            logger.LogWarning("GetPracticesByLanguageQueryHandler -> LANGUAGE NOT FOUND: {Language}", language);
            return ServiceResult<List<PracticeDto>>.Fail($"LANGUAGE '{language}' NOT FOUND.",
                HttpStatusCode.NotFound);
        }

        var practices = await practiceRepository.GetPracticesByLanguageAsync(language);

        logger.LogInformation("GetPracticesByLanguageQueryHandler -> SUCCESSFULLY FETCHED {Count} PRACTICES FOR LANGUAGE: {Language}", practices.Count, language);

        var result = mapper.Map<List<PracticeDto>>(practices);

        return ServiceResult<List<PracticeDto>>.Success(result);
    }
}
