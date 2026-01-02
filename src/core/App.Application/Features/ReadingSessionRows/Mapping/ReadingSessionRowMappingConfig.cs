using Mapster;
using App.Domain.Entities.ReadingEntities;
using App.Application.Features.ReadingSessionRows.Dtos;

namespace App.Application.Features.ReadingSessionRows.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR READING SESSION ROW ENTITY MAPPINGS.
/// </summary>
public class ReadingSessionRowMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReadingSessionRow, ReadingSessionRowDto>();
    }
}
