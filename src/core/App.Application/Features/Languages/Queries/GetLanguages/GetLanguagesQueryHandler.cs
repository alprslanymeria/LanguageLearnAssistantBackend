using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Languages.Dtos;
using MapsterMapper;

namespace App.Application.Features.Languages.Queries.GetLanguages;

/// <summary>
/// HANDLER FOR GET LANGUAGES QUERY.
/// </summary>
public class GetLanguagesQueryHandler(

    ILanguageRepository languageRepository,
    IMapper mapper

    ) : IQueryHandler<GetLanguagesQuery, ServiceResult<List<LanguageDto>>>
{

    public async Task<ServiceResult<List<LanguageDto>>> Handle(

        GetLanguagesQuery request,
        CancellationToken cancellationToken)
    {
        var languages = await languageRepository.GetLanguagesAsync();

        if (languages.Count == 0)
        {
            return ServiceResult<List<LanguageDto>>.Success([]);
        }

        var result = mapper.Map<List<LanguageDto>>(languages);

        return ServiceResult<List<LanguageDto>>.Success(result);
    }
}
