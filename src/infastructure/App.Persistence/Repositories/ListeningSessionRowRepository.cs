using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LISTENING SESSION ROW ENTITY.
/// </summary>
public class ListeningSessionRowRepository(AppDbContext context) : GenericRepository<ListeningSessionRow, int>(context), IListeningSessionRowRepository
{
    public async Task<(List<ListeningSessionRow> items, int totalCount)> GetListeningRowsByIdWithPagingAsync(string sessionId, int page, int pageSize)
    {
        var query = Context.ListeningSessionRows
            .AsNoTracking()
            .Where(l =>l.ListeningOldSessionId == sessionId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(l => l.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task CreateRangeAsync(IEnumerable<ListeningSessionRow> rows)
    {
        await Context.ListeningSessionRows.AddRangeAsync(rows);
    }
}
