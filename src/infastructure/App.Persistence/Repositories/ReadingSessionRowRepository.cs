using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR READING SESSION ROW ENTITY.
/// </summary>
public class ReadingSessionRowRepository(AppDbContext context) : IReadingSessionRowRepository
{
    public async Task AddAsync(ReadingSessionRow entity) => await context.ReadingSessionRows.AddAsync(entity);

    public async Task<ReadingSessionRow?> GetByIdAsync(int id) =>
        await context.ReadingSessionRows
            .AsNoTracking()
            .FirstOrDefaultAsync(rsr => rsr.Id == id);

    public void Update(ReadingSessionRow entity) => context.ReadingSessionRows.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.ReadingSessionRows.FindAsync(id);

        if (entity is not null)
        {
            context.ReadingSessionRows.Remove(entity);
        }
    }

    public async Task<(List<ReadingSessionRow> Items, int TotalCount)> GetReadingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize)
    {
        var query = context.ReadingSessionRows
            .Where(rsr => rsr.ReadingOldSessionId == sessionId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(rsr => rsr.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddRangeAsync(IEnumerable<ReadingSessionRow> rows) =>
        await context.ReadingSessionRows.AddRangeAsync(rows);
}
