using App.Application.Features.WritingBooks.Dtos;
using App.Domain.Entities.WritingEntities;
using Mapster;

namespace App.Application.Features.WritingBooks.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR WRITING BOOK ENTITY MAPPINGS.
/// </summary>
public class WritingBookMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<WritingBook, WritingBookDto>();
        config.NewConfig<WritingBook, WritingBookWithLanguageId>();
        config.NewConfig<List<WritingBook>, List<WritingBookDto>>();
    }
}
