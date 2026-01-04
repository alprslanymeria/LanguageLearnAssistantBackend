using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LISTENING ENTITY.
/// </summary>
public class ListeningRepository(AppDbContext context) : IListeningRepository
{
    public async Task CreateAsync(Listening entity) => await context.Listenings.AddAsync(entity);

    public Task<Listening?> GetByIdAsync(int id) =>
        context.Listenings
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

    public Listening Update(Listening entity)
    {
        context.Listenings.Update(entity);

        return entity;
    }

    public void Delete(Listening entity)
    {
        context.Listenings
            .Remove(entity);
    }
}
