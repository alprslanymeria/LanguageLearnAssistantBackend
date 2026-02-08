using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Practices.Dtos;
using App.Domain.Exceptions;
using MapsterMapper;

namespace App.Application.Features.Practices.Queries.GetPracticesByLanguage;

/// <summary>
/// HANDLER FOR GET PRACTICES BY LANGUAGE QUERY.
/// </summary>
public class GetPracticesByLanguageQueryHandler(

    IPracticeRepository practiceRepository,
    ILanguageRepository languageRepository,
    IMapper mapper

    ) : IQueryHandler<GetPracticesByLanguageQuery, ServiceResult<List<PracticeDto>>>
{
    public async Task<ServiceResult<List<PracticeDto>>> Handle(

        GetPracticesByLanguageQuery request,
        CancellationToken cancellationToken)
    {
        var language = request.Language.Trim().ToLower();

        // CHECK IF LANGUAGES EXIST
        _ = await languageRepository.ExistsByNameAsync(language)
            ?? throw new NotFoundException($"LANGUAGE '{language}' NOT FOUND.");

        var practices = await practiceRepository.GetPracticesByLanguageAsync(language);

        var result = mapper.Map<List<PracticeDto>>(practices);

        return ServiceResult<List<PracticeDto>>.Success(result);
    }
}
