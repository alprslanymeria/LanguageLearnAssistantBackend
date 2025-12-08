using App.Application.Features.Readings.Dtos;
using App.Domain.Entities.ReadingEntities;
using Mapster;

namespace App.Infrastructure.Mapping.Readings;

public class ReadingMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Reading, ReadingDto>()
            .Map(dest => dest.LanguageName, src => src.Language.Name)
            .Map(dest => dest.PracticeName, src => src.Practice.Name);
        
        config.NewConfig<CreateReadingDto, Reading>();
        config.NewConfig<UpdateReadingDto, Reading>();
    }
}
