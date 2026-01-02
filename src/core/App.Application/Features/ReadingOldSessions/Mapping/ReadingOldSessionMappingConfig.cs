using Mapster;
using App.Domain.Entities.ReadingEntities;
using App.Application.Features.ReadingOldSessions.Dtos;

namespace App.Application.Features.ReadingOldSessions.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR READING OLD SESSION ENTITY MAPPINGS.
/// </summary>
public class ReadingOldSessionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReadingOldSession, ReadingOldSessionDto>();
        config.NewConfig<List<ReadingOldSession>, List<ReadingOldSessionDto>>();
    }
}
