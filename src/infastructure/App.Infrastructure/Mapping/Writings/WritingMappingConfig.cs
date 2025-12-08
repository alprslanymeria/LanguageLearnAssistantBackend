using App.Application.Features.Writings.Dtos;
using App.Domain.Entities.WritingEntities;
using Mapster;

namespace App.Infrastructure.Mapping.Writings;

public class WritingMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Writing, WritingDto>()
            .Map(dest => dest.LanguageName, src => src.Language.Name)
            .Map(dest => dest.PracticeName, src => src.Practice.Name);
        
        config.NewConfig<CreateWritingDto, Writing>();
        config.NewConfig<UpdateWritingDto, Writing>();
    }
}
