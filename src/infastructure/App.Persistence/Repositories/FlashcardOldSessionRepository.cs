using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR FLASHCARD OLD SESSION ENTITY.
/// </summary>
public class FlashcardOldSessionRepository(AppDbContext context) : IFlashcardOldSessionRepository
{
    public async Task AddAsync(FlashcardOldSession entity) => await context.FlashcardOldSessions.AddAsync(entity);

    public async Task<FlashcardOldSession?> GetByIdAsync(string id) =>
        await context.FlashcardOldSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(fos => fos.Id == id);

    public void Update(FlashcardOldSession entity) => context.FlashcardOldSessions.Update(entity);

    public async Task RemoveAsync(string id)
    {
        var entity = await context.FlashcardOldSessions.FindAsync(id);

        if (entity is not null)
        {
            context.FlashcardOldSessions.Remove(entity);
        }
    }

    public async Task<(List<FlashcardOldSession> Items, int TotalCount)> GetFlashcardOldSessionsWithPagingAsync(string userId, string language, int page, int pageSize)
    {
        var query = context.FlashcardOldSessions
            .Where(fos => fos.Flashcard.UserId == userId && fos.Flashcard.Language.Name == language);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(fos => fos.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
