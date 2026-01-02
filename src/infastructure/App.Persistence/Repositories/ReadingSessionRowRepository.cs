using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR READING SESSION ROW ENTITY.
/// </summary>
public class ReadingSessionRowRepository(AppDbContext context) : GenericRepository<ReadingSessionRow, int>(context), IReadingSessionRowRepository
{
    public async Task<(List<ReadingSessionRow> items, int totalCount)> GetReadingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize)
    {
        var query = Context.ReadingSessionRows
            .AsNoTracking()
            .Where(r => r.ReadingOldSessionId == sessionId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(r => r.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task CreateRangeAsync(IEnumerable<ReadingSessionRow> rows)
    {
        await Context.ReadingSessionRows.AddRangeAsync(rows);
    }
}