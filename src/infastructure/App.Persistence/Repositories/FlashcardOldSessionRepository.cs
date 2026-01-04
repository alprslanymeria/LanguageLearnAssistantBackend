using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR FLASHCARD OLD SESSION ENTITY.
/// </summary>
public class FlashcardOldSessionRepository(AppDbContext context) : IFlashcardOldSessionRepository
{
    public async Task<(List<FlashcardOldSession> items, int totalCount)> GetFlashcardOldSessionsWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = context.FlashcardOldSessions
            .AsNoTracking()
            .Where(fos => fos.Flashcard.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(fos => fos.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task CreateAsync(FlashcardOldSession entity) => await context.FlashcardOldSessions.AddAsync(entity);

    public Task<FlashcardOldSession?> GetByIdAsync(string id) =>
        context.FlashcardOldSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(fos => fos.Id == id);

    public FlashcardOldSession Update(FlashcardOldSession entity)
    {
        context.FlashcardOldSessions.Update(entity);

        return entity;
    }

    public void Delete(FlashcardOldSession entity)
    {
        context.FlashcardOldSessions
            .Remove(entity);
    }
}
