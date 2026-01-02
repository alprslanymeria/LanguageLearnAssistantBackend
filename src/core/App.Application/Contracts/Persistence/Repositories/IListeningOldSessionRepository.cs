using App.Domain.Entities.ListeningEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR LISTENING OLD SESSION ENTITY.
/// </summary>
public interface IListeningOldSessionRepository : IGenericRepository<ListeningOldSession, string>
{
    /// <summary>
    /// GETS ALL LISTENING OLD SESSIONS FOR A USER WITH DETAILS.
    /// </summary>
    Task<(List<ListeningOldSession> items, int totalCount)> GetListeningOldSessionsWithPagingAsync(string userId, int page, int pageSize);
}
