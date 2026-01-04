using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LISTENING OLD SESSION ENTITY.
/// </summary>
public class ListeningOldSessionRepository(AppDbContext context) : IListeningOldSessionRepository
{
    public async Task<(List<ListeningOldSession> items, int totalCount)> GetListeningOldSessionsWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = context.ListeningOldSessions
            .AsNoTracking()
            .Where(los => los.Listening.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(los => los.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task CreateAsync(ListeningOldSession entity) => await context.ListeningOldSessions.AddAsync(entity);

    public Task<ListeningOldSession?> GetByIdAsync(string id) =>
        context.ListeningOldSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(los => los.Id == id);

    public ListeningOldSession Update(ListeningOldSession entity)
    {
        context.ListeningOldSessions.Update(entity);

        return entity;
    }

    public void Delete(ListeningOldSession entity)
    {
        context.ListeningOldSessions
            .Remove(entity);
    }
}
