using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR READING ENTITY.
/// </summary>
public interface IReadingRepository : IGenericRepository<Reading, int>
{
}
