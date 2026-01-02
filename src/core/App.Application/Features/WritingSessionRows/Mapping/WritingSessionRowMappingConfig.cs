using Mapster;
using App.Domain.Entities.WritingEntities;
using App.Application.Features.WritingSessionRows.Dtos;

namespace App.Application.Features.WritingSessionRows.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR WRITING SESSION ROW ENTITY MAPPINGS.
/// </summary>
public class WritingSessionRowMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<WritingSessionRow, WritingSessionRowDto>();
    }
}
