using Mapster;
using App.Domain.Entities.ListeningEntities;
using App.Application.Features.ListeningSessionRows.Dtos;

namespace App.Application.Features.ListeningSessionRows.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR LISTENING SESSION ROW ENTITY MAPPINGS.
/// </summary>
public class ListeningSessionRowMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ListeningSessionRow, ListeningSessionRowDto>();
    }
}
