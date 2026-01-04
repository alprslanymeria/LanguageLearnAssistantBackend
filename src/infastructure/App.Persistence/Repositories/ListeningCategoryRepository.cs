using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LISTENING CATEGORY ENTITY.
/// </summary>
public class ListeningCategoryRepository(AppDbContext context) : IListeningCategoryRepository
{
    public async Task CreateAsync(ListeningCategory entity) => await context.ListeningCategories.AddAsync(entity);

    public Task<ListeningCategory?> GetByIdAsync(int id) =>
        context.ListeningCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(lc => lc.Id == id);

    public ListeningCategory Update(ListeningCategory entity)
    {
        context.ListeningCategories.Update(entity);

        return entity;
    }

    public void Delete(ListeningCategory entity)
    {
        context.ListeningCategories
            .Remove(entity);
    }
}
