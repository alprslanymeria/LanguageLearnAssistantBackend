using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR FLASHCARD SESSION ROW ENTITY.
/// </summary>
public class FlashcardSessionRowRepository(AppDbContext context) : IFlashcardSessionRowRepository
{
    public async Task<(List<FlashcardSessionRow> items, int totalCount)> GetFlashcardRowsByIdWithPagingAsync(string sessionId, int page, int pageSize)
    {
        var query = context.FlashcardSessionRows
            .AsNoTracking()
            .Where(r => r.FlashcardOldSessionId == sessionId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(f => f.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task CreateRangeAsync(IEnumerable<FlashcardSessionRow> rows)
    {
        await context.FlashcardSessionRows.AddRangeAsync(rows);
    }

    public async Task CreateAsync(FlashcardSessionRow entity) => await context.FlashcardSessionRows.AddAsync(entity);

    public Task<FlashcardSessionRow?> GetByIdAsync(int id) =>
        context.FlashcardSessionRows
            .AsNoTracking()
            .FirstOrDefaultAsync(fsr => fsr.Id == id);

    public FlashcardSessionRow Update(FlashcardSessionRow entity)
    {
        context.FlashcardSessionRows.Update(entity);

        return entity;
    }

    public void Delete(FlashcardSessionRow entity)
    {
        context.FlashcardSessionRows
            .Remove(entity);
    }
}
