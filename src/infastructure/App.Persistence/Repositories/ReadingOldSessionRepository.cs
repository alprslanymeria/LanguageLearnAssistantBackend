using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR READING OLD SESSION ENTITY.
/// </summary>
public class ReadingOldSessionRepository(AppDbContext context) : IReadingOldSessionRepository
{
    
    public async Task<(List<ReadingOldSession> items, int totalCount)> GetReadingOldSessionsWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = context.ReadingOldSessions
            .AsNoTracking()
            .Where(ros => ros.Reading.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(ros => ros.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task CreateAsync(ReadingOldSession entity) => await context.ReadingOldSessions.AddAsync(entity);

    public Task<ReadingOldSession?> GetByIdAsync(string id) =>
        context.ReadingOldSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(ros => ros.Id == id);

    public ReadingOldSession Update(ReadingOldSession entity)
    {
        context.ReadingOldSessions.Update(entity);

        return entity;
    }

    public void Delete(ReadingOldSession entity)
    {
        context.ReadingOldSessions
            .Remove(entity);
    }
}
