using App.Application.Features.Practices.Dtos;
using App.Domain.Entities;
using Mapster;

namespace App.Infrastructure.Mapping.Practices;

public class PracticeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Practice, PracticeDto>()
            .Map(dest => dest.LanguageName, src => src.Language.Name);
        
        config.NewConfig<CreatePracticeDto, Practice>();
        config.NewConfig<UpdatePracticeDto, Practice>();
    }
}
