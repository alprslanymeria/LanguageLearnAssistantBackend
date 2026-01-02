using Mapster;
using App.Domain.Entities.FlashcardEntities;
using App.Application.Features.DeckWords.Dtos;

namespace App.Application.Features.DeckWords.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR DECK WORD ENTITY MAPPINGS.
/// </summary>
public class DeckWordMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DeckWord, DeckWordDto>();
        config.NewConfig<DeckWord, DeckWordWithLanguageId>();
    }
}
