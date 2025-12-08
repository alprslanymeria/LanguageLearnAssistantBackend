using App.Application.Features.Flashcards.Dtos;
using App.Domain.Entities.FlashcardEntities;
using Mapster;

namespace App.Infrastructure.Mapping.Flashcards;

public class FlashcardMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Flashcard, FlashcardDto>()
            .Map(dest => dest.LanguageName, src => src.Language.Name)
            .Map(dest => dest.PracticeName, src => src.Practice.Name);
        
        config.NewConfig<CreateFlashcardDto, Flashcard>();
        config.NewConfig<UpdateFlashcardDto, Flashcard>();
    }
}
