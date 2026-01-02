using Mapster;
using App.Domain.Entities;
using App.Application.Features.Languages.Dtos;

namespace App.Application.Features.Languages.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR LANGUAGE ENTITY MAPPINGS.
/// </summary>
public class LanguageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<List<Language>, List<LanguageDto>>();
    }
}
