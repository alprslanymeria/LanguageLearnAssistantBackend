using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LISTENING SESSION ROW ENTITY.
/// </summary>
public class ListeningSessionRowRepository(AppDbContext context) : IListeningSessionRowRepository
{
    public async Task<(List<ListeningSessionRow> items, int totalCount)> GetListeningRowsByIdWithPagingAsync(string sessionId, int page, int pageSize)
    {
        var query = context.ListeningSessionRows
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
        await context.ListeningSessionRows.AddRangeAsync(rows);
    }

    public async Task CreateAsync(ListeningSessionRow entity) => await context.ListeningSessionRows.AddAsync(entity);

    public Task<ListeningSessionRow?> GetByIdAsync(int id) =>
        context.ListeningSessionRows
            .AsNoTracking()
            .FirstOrDefaultAsync(lsr => lsr.Id == id);

    public ListeningSessionRow Update(ListeningSessionRow entity)
    {
        context.ListeningSessionRows.Update(entity);

        return entity;
    }

    public void Delete(ListeningSessionRow entity)
    {
        context.ListeningSessionRows
            .Remove(entity);
    }
}
