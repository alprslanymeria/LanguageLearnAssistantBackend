using Mapster;
using App.Domain.Entities.FlashcardEntities;
using App.Application.Features.FlashcardOldSessions.Dtos;

namespace App.Application.Features.FlashcardOldSessions.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR FLASHCARD OLD SESSION ENTITY MAPPINGS.
/// </summary>
public class FlashcardOldSessionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FlashcardOldSession, FlashcardOldSessionDto>();
        config.NewConfig<List<FlashcardOldSession>, List<FlashcardOldSessionDto>>();    
    }
}
