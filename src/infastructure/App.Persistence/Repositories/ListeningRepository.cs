using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LISTENING ENTITY.
/// </summary>
public class ListeningRepository(AppDbContext context) : GenericRepository<Listening, int>(context), IListeningRepository
{
}
