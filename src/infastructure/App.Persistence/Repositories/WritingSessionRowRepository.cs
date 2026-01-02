using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR WRITING SESSION ROW ENTITY.
/// </summary>
public class WritingSessionRowRepository(AppDbContext context) : GenericRepository<WritingSessionRow, int>(context), IWritingSessionRowRepository
{
    public async Task<(List<WritingSessionRow> items, int totalCount)> GetWritingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize)
    {
        var query = Context.WritingSessionRows
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
        await Context.WritingSessionRows.AddRangeAsync(rows);
    }
}
