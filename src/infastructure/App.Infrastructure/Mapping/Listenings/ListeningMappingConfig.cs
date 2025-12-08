using App.Application.Features.Listenings.Dtos;
using App.Domain.Entities.ListeningEntities;
using Mapster;

namespace App.Infrastructure.Mapping.Listenings;

public class ListeningMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Listening, ListeningDto>()
            .Map(dest => dest.LanguageName, src => src.Language.Name)
            .Map(dest => dest.PracticeName, src => src.Practice.Name);
        
        config.NewConfig<CreateListeningDto, Listening>();
        config.NewConfig<UpdateListeningDto, Listening>();
    }
}
