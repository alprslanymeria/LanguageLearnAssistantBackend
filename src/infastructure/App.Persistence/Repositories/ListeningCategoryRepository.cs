using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LISTENING CATEGORY ENTITY.
/// </summary>
public class ListeningCategoryRepository(AppDbContext context) : GenericRepository<ListeningCategory, int>(context), IListeningCategoryRepository
{

}
