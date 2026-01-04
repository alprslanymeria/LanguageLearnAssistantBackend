using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR READING SESSION ROW ENTITY.
/// </summary>
public class ReadingSessionRowRepository(AppDbContext context) : IReadingSessionRowRepository
{
    public async Task<(List<ReadingSessionRow> items, int totalCount)> GetReadingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize)
    {
        var query = context.ReadingSessionRows
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
        await context.ReadingSessionRows.AddRangeAsync(rows);
    }

    public async Task CreateAsync(ReadingSessionRow entity) => await context.ReadingSessionRows.AddAsync(entity);

    public Task<ReadingSessionRow?> GetByIdAsync(int id) =>
        context.ReadingSessionRows
            .AsNoTracking()
            .FirstOrDefaultAsync(rsr => rsr.Id == id);

    public ReadingSessionRow Update(ReadingSessionRow entity)
    {
        context.ReadingSessionRows.Update(entity);

        return entity;
    }

    public void Delete(ReadingSessionRow entity)
    {
        context.ReadingSessionRows
            .Remove(entity);
    }
}
