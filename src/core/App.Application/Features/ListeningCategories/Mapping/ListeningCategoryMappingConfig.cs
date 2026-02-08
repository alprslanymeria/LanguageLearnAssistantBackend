using App.Application.Features.ListeningCategories.Dtos;
using App.Domain.Entities.ListeningEntities;
using Mapster;

namespace App.Application.Features.ListeningCategories.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR LISTENING CATEGORY ENTITY MAPPINGS.
/// </summary>
public class ListeningCategoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ListeningCategory, ListeningCategoryDto>();
        config.NewConfig<List<ListeningCategory>, List<ListeningCategoryDto>>();
    }
}
