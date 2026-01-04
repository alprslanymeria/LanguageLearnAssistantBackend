using App.Domain.Entities.ListeningEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR LISTENING OLD SESSION ENTITY.
/// </summary>
public interface IListeningOldSessionRepository
{
    /// <summary>
    /// GETS ALL LISTENING OLD SESSIONS FOR A USER WITH DETAILS.
    /// </summary>
    Task<(List<ListeningOldSession> items, int totalCount)> GetListeningOldSessionsWithPagingAsync(string userId, int page, int pageSize);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(ListeningOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ListeningOldSession?> GetByIdAsync(string id);

    /// <summary>
    /// UPDATES THE LISTENING OLD SESSION IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    ListeningOldSession Update(ListeningOldSession entity);

    /// <summary>
    /// REMOVES THE LISTENING OLD SESSION FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(ListeningOldSession entity);
}
