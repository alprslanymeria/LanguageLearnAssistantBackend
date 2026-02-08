using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckVideo.Dtos;
using App.Application.Features.ListeningCategories.Dtos;
using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LISTENING CATEGORY ENTITY.
/// </summary>
public class ListeningCategoryRepository(AppDbContext context) : IListeningCategoryRepository
{
    public async Task AddAsync(ListeningCategory entity) => await context.ListeningCategories.AddAsync(entity);

    public async Task<ListeningCategory?> GetByIdAsync(int id) =>
        await context.ListeningCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(lc => lc.Id == id);

    public void Update(ListeningCategory entity) => context.ListeningCategories.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.ListeningCategories.FindAsync(id);

        if (entity is not null)
        {
            context.ListeningCategories.Remove(entity);
        }
    }

    public async Task<List<ListeningCategoryWithDeckVideos>> GetLCategoryCreateItemsAsync(string userId, int languageId, int practiceId)
    {
        return await context.ListeningCategories
            .Where(lc =>
                lc.Listening.UserId == userId &&
                lc.Listening.LanguageId == languageId &&
                lc.Listening.PracticeId == practiceId &&
                lc.DeckVideos.Any())
            .Select(lc => new ListeningCategoryWithDeckVideos(
                lc.Id,
                lc.Name,
                lc.ListeningId,
                lc.DeckVideos
                    .Select(dv => new DeckVideoDto(
                        dv.Id,
                        dv.ListeningCategoryId,
                        dv.Correct,
                        dv.SourceUrl
                    ))
                    .ToList()
            ))
            .ToListAsync();
    }

    public async Task<ListeningCategoryWithDeckVideos?> GetByIdWithDeckVideosAsync(int id)
    {
        return await context.ListeningCategories
            .Where(lc => lc.Id == id)
            .Select(lc => new ListeningCategoryWithDeckVideos(
                lc.Id,
                lc.Name,
                lc.ListeningId,
                lc.DeckVideos
                    .Select(dv => new DeckVideoDto(
                        dv.Id,
                        dv.ListeningCategoryId,
                        dv.Correct,
                        dv.SourceUrl
                    ))
                    .ToList()
            ))
            .FirstOrDefaultAsync();
    }
}
