using App.Application.Features.Languages.Dtos;
using App.Domain.Entities;
using Mapster;

namespace App.Infrastructure.Mapping.Languages;

public class LanguageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Language, LanguageDto>();
        config.NewConfig<CreateLanguageDto, Language>();
        config.NewConfig<UpdateLanguageDto, Language>();
    }
}
