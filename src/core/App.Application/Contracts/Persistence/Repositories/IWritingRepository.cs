using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR WRITING ENTITY.
/// </summary>
public interface IWritingRepository : IGenericRepository<Writing, int>
{
}
