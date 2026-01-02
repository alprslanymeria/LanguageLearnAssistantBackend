using Mapster;
using App.Domain.Entities;
using App.Application.Features.Practices.Dtos;

namespace App.Application.Features.Practices.Mapping;

/// <summary>
/// MAPSTER CONFIGURATION FOR PRACTICE ENTITY MAPPINGS.
/// </summary>
public class PracticeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<List<Practice>, List<PracticeDto>>();
    }
}
