using App.Domain.Entities.ReadingEntities;
using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR WRITING OLD SESSION ENTITY.
/// </summary>
public interface IWritingOldSessionRepository : IGenericRepository<WritingOldSession, string>
{
    /// <summary>
    /// GETS ALL WRITING OLD SESSIONS FOR A USER WITH DETAILS.
    /// </summary>
    Task<(List<WritingOldSession> items, int totalCount)> GetWritingOldSessionsWithPagingAsync(string userId, int page, int pageSize);
}