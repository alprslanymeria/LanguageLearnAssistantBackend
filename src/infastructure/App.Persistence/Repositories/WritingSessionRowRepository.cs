using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR WRITING SESSION ROW ENTITY.
/// </summary>
public class WritingSessionRowRepository(AppDbContext context) : IWritingSessionRowRepository
{
    public async Task<(List<WritingSessionRow> items, int totalCount)> GetWritingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize)
    {
        var query = context.WritingSessionRows
            .AsNoTracking()
            .Where(r => r.WritingOldSessionId == sessionId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(w => w.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task CreateRangeAsync(IEnumerable<WritingSessionRow> rows)
    {
        await context.WritingSessionRows.AddRangeAsync(rows);
    }

    public async Task CreateAsync(WritingSessionRow entity) => await context.WritingSessionRows.AddAsync(entity);

    public Task<WritingSessionRow?> GetByIdAsync(int id) =>
        context.WritingSessionRows
            .AsNoTracking()
            .FirstOrDefaultAsync(wsr => wsr.Id == id);

    public WritingSessionRow Update(WritingSessionRow entity)
    {
        context.WritingSessionRows.Update(entity);

        return entity;
    }

    public void Delete(WritingSessionRow entity)
    {
        context.WritingSessionRows
            .Remove(entity);
    }
}
