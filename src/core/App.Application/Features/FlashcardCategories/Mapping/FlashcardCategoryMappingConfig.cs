using Mapster;
using App.Domain.Entities.FlashcardEntities;
using App.Application.Features.FlashcardCategories.Dtos;

namespace App.Application.Features.FlashcardCategories.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR FLASHCARD CATEGORY ENTITY MAPPINGS.
/// </summary>
public class FlashcardCategoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FlashcardCategory, FlashcardCategoryDto>();
        config.NewConfig<FlashcardCategory, FlashcardCategoryWithLanguageId>();
        config.NewConfig<List<FlashcardCategory>, List<FlashcardCategoryDto>>();
    }
}
