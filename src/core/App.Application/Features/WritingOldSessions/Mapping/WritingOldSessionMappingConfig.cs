using Mapster;
using App.Domain.Entities.WritingEntities;
using App.Application.Features.WritingOldSessions.Dtos;

namespace App.Application.Features.WritingOldSessions.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR WRITING OLD SESSION ENTITY MAPPINGS.
/// </summary>
public class WritingOldSessionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<WritingOldSession, WritingOldSessionDto>();
        config.NewConfig<List<WritingOldSession>, List<WritingOldSessionDto>>();
    }
}
