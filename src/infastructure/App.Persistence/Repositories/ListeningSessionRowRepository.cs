using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LISTENING SESSION ROW ENTITY.
/// </summary>
public class ListeningSessionRowRepository(AppDbContext context) : IListeningSessionRowRepository
{
    public async Task AddAsync(ListeningSessionRow entity) => await context.ListeningSessionRows.AddAsync(entity);

    public async Task<ListeningSessionRow?> GetByIdAsync(int id) =>
        await context.ListeningSessionRows
            .AsNoTracking()
            .FirstOrDefaultAsync(lsr => lsr.Id == id);

    public void Update(ListeningSessionRow entity) => context.ListeningSessionRows.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.ListeningSessionRows.FindAsync(id);

        if (entity is not null)
        {
            context.ListeningSessionRows.Remove(entity);
        }
    }

    public async Task<(List<ListeningSessionRow> Items, int TotalCount)> GetListeningRowsByIdWithPagingAsync(string sessionId, int page, int pageSize)
    {
        var query = context.ListeningSessionRows
            .Where(lsr => lsr.ListeningOldSessionId == sessionId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(lsr => lsr.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddRangeAsync(IEnumerable<ListeningSessionRow> rows) =>
        await context.ListeningSessionRows.AddRangeAsync(rows);
}
