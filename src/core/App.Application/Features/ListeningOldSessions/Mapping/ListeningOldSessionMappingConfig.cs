using Mapster;
using App.Domain.Entities.ListeningEntities;
using App.Application.Features.ListeningOldSessions.Dtos;

namespace App.Application.Features.ListeningOldSessions.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR LISTENING OLD SESSION ENTITY MAPPINGS.
/// </summary>
public class ListeningOldSessionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ListeningOldSession, ListeningOldSessionDto>();
        config.NewConfig<List<ListeningOldSession>, List<ListeningOldSessionDto>>();
    }
}
