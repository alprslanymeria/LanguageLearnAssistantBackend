using App.Application.Features.Languages.DTOs;
using App.Domain.Entities;
using Mapster;

namespace App.Application.Features.Languages.Mappings;

public class LanguageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Language, LanguageDto>();
    }
}
