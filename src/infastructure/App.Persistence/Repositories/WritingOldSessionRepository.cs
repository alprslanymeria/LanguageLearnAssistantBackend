using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR WRITING OLD SESSION ENTITY.
/// </summary>
public class WritingOldSessionRepository(AppDbContext context) : IWritingOldSessionRepository
{
    public async Task AddAsync(WritingOldSession entity) => await context.WritingOldSessions.AddAsync(entity);

    public async Task<WritingOldSession?> GetByIdAsync(string id) =>
        await context.WritingOldSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(wos => wos.Id == id);

    public void Update(WritingOldSession entity) => context.WritingOldSessions.Update(entity);

    public async Task RemoveAsync(string id)
    {
        var entity = await context.WritingOldSessions.FindAsync(id);

        if (entity is not null)
        {
            context.WritingOldSessions.Remove(entity);
        }
    }

    public async Task<(List<WritingOldSession> Items, int TotalCount)> GetWritingOldSessionsWithPagingAsync(string userId, string language, int page, int pageSize)
    {
        var query = context.WritingOldSessions
            .Where(wos => wos.Writing.UserId == userId && wos.Writing.Language.Name == language);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(wos => wos.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
