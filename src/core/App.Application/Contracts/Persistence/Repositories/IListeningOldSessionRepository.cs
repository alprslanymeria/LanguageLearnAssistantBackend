using App.Domain.Entities.ListeningEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IListeningOldSessionRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(ListeningOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ListeningOldSession?> GetByIdAsync(string id);

    /// <summary>
    /// UPDATES THE LISTENING OLD SESSION IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(ListeningOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE LISTENING OLD SESSION FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(string id);

    /// <summary>
    /// GETS ALL LISTENING OLD SESSIONS FOR A USER WITH PAGING AND LANGUAGE FILTER.
    /// </summary>
    Task<(List<ListeningOldSession> Items, int TotalCount)> GetListeningOldSessionsWithPagingAsync(string userId, string language, int page, int pageSize);
}
