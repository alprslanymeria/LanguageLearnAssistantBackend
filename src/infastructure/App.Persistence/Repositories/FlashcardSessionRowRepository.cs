using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR FLASHCARD SESSION ROW ENTITY.
/// </summary>
public class FlashcardSessionRowRepository(AppDbContext context) : IFlashcardSessionRowRepository
{
    public async Task AddAsync(FlashcardSessionRow entity) => await context.FlashcardSessionRows.AddAsync(entity);

    public async Task<FlashcardSessionRow?> GetByIdAsync(int id) =>
        await context.FlashcardSessionRows
            .AsNoTracking()
            .FirstOrDefaultAsync(fsr => fsr.Id == id);

    public void Update(FlashcardSessionRow entity) => context.FlashcardSessionRows.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.FlashcardSessionRows.FindAsync(id);

        if (entity is not null)
        {
            context.FlashcardSessionRows.Remove(entity);
        }
    }

    public async Task<(List<FlashcardSessionRow> Items, int TotalCount)> GetFlashcardRowsByIdWithPagingAsync(string sessionId, int page, int pageSize)
    {
        var query = context.FlashcardSessionRows
            .Where(fsr => fsr.FlashcardOldSessionId == sessionId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(fsr => fsr.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddRangeAsync(IEnumerable<FlashcardSessionRow> rows) =>
        await context.FlashcardSessionRows.AddRangeAsync(rows);
}
