using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities; 
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR WRITING OLD SESSION ENTITY.
/// </summary>
public class WritingOldSessionRepository(AppDbContext context) : GenericRepository<WritingOldSession, string>(context), IWritingOldSessionRepository
{
    public async Task<(List<WritingOldSession> items, int totalCount)> GetWritingOldSessionsWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = Context.WritingOldSessions
            .AsNoTracking()
            .Where(wos => wos.Writing.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(wos => wos.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
