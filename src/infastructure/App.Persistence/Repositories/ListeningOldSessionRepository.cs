using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LISTENING OLD SESSION ENTITY.
/// </summary>
public class ListeningOldSessionRepository(AppDbContext context) : IListeningOldSessionRepository
{
    public async Task AddAsync(ListeningOldSession entity) => await context.ListeningOldSessions.AddAsync(entity);

    public async Task<ListeningOldSession?> GetByIdAsync(string id) =>
        await context.ListeningOldSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(los => los.Id == id);

    public void Update(ListeningOldSession entity) => context.ListeningOldSessions.Update(entity);

    public async Task RemoveAsync(string id)
    {
        var entity = await context.ListeningOldSessions.FindAsync(id);

        if (entity is not null)
        {
            context.ListeningOldSessions.Remove(entity);
        }
    }

    public async Task<(List<ListeningOldSession> Items, int TotalCount)> GetListeningOldSessionsWithPagingAsync(string userId, string language, int page, int pageSize)
    {
        var query = context.ListeningOldSessions
            .Where(los => los.Listening.UserId == userId && los.Listening.Language.Name == language);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(los => los.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
