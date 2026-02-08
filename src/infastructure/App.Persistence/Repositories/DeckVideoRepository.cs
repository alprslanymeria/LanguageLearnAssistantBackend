using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR DECK VIDEO ENTITY.
/// </summary>
public class DeckVideoRepository(AppDbContext context) : IDeckVideoRepository
{
    public async Task AddAsync(DeckVideo entity) => await context.DeckVideos.AddAsync(entity);

    public async Task<DeckVideo?> GetByIdAsync(int id) =>
        await context.DeckVideos
            .AsNoTracking()
            .FirstOrDefaultAsync(dv => dv.Id == id);

    public void Update(DeckVideo entity) => context.DeckVideos.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.DeckVideos.FindAsync(id);

        if (entity is not null)
        {
            context.DeckVideos.Remove(entity);
        }
    }
}
