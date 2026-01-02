using Mapster;
using App.Domain.Entities.FlashcardEntities;
using App.Application.Features.FlashcardSessionRows.Dtos;

namespace App.Application.Features.FlashcardSessionRows.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR FLASHCARD SESSION ROW ENTITY MAPPINGS.
/// </summary>
public class FlashcardSessionRowMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FlashcardSessionRow, FlashcardSessionRowDto>();
    }
}
