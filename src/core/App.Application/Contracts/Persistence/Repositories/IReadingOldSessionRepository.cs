using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR READING OLD SESSION ENTITY.
/// </summary>
public interface IReadingOldSessionRepository : IGenericRepository<ReadingOldSession, string>
{
    /// <summary>
    /// GETS ALL READING OLD SESSIONS FOR A USER WITH DETAILS.
    /// </summary>
    Task<(List<ReadingOldSession> items, int totalCount)> GetReadingOldSessionsWithPagingAsync(string userId, int page, int pageSize);
}