using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR WRITING SESSION ROW ENTITY.
/// </summary>
public class WritingSessionRowRepository(AppDbContext context) : IWritingSessionRowRepository
{
    public async Task AddAsync(WritingSessionRow entity) => await context.WritingSessionRows.AddAsync(entity);

    public async Task<WritingSessionRow?> GetByIdAsync(int id) =>
        await context.WritingSessionRows
            .AsNoTracking()
            .FirstOrDefaultAsync(wsr => wsr.Id == id);

    public void Update(WritingSessionRow entity) => context.WritingSessionRows.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.WritingSessionRows.FindAsync(id);

        if (entity is not null)
        {
            context.WritingSessionRows.Remove(entity);
        }
    }

    public async Task<(List<WritingSessionRow> Items, int TotalCount)> GetWritingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize)
    {
        var query = context.WritingSessionRows
            .Where(wsr => wsr.WritingOldSessionId == sessionId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(wsr => wsr.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddRangeAsync(IEnumerable<WritingSessionRow> rows) =>
        await context.WritingSessionRows.AddRangeAsync(rows);
}
