using Mapster;
using App.Domain.Entities.ReadingEntities;
using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingBooks.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR READING BOOK ENTITY MAPPINGS.
/// </summary>
public class ReadingBookMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ReadingBook, ReadingBookDto>();
        config.NewConfig<ReadingBook, ReadingBookWithLanguageId>();
        config.NewConfig<List<ReadingBook>, List<ReadingBookDto>>();
    }
}
