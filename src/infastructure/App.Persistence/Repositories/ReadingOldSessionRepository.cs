using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR READING OLD SESSION ENTITY.
/// </summary>
public class ReadingOldSessionRepository(AppDbContext context) : IReadingOldSessionRepository
{
    public async Task AddAsync(ReadingOldSession entity) => await context.ReadingOldSessions.AddAsync(entity);

    public async Task<ReadingOldSession?> GetByIdAsync(string id) =>
        await context.ReadingOldSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(ros => ros.Id == id);

    public void Update(ReadingOldSession entity) => context.ReadingOldSessions.Update(entity);

    public async Task RemoveAsync(string id)
    {
        var entity = await context.ReadingOldSessions.FindAsync(id);

        if (entity is not null)
        {
            context.ReadingOldSessions.Remove(entity);
        }
    }

    public async Task<(List<ReadingOldSession> Items, int TotalCount)> GetReadingOldSessionsWithPagingAsync(string userId, string language, int page, int pageSize)
    {
        var query = context.ReadingOldSessions
            .Where(ros => ros.Reading.UserId == userId && ros.Reading.Language.Name == language);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(ros => ros.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
