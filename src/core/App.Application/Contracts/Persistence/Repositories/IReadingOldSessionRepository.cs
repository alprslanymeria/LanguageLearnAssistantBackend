using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IReadingOldSessionRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(ReadingOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ReadingOldSession?> GetByIdAsync(string id);

    /// <summary>
    /// UPDATES THE READING OLD SESSION IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(ReadingOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE READING OLD SESSION FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(string id);

    /// <summary>
    /// GETS ALL READING OLD SESSIONS FOR A USER WITH PAGING AND LANGUAGE FILTER.
    /// </summary>
    Task<(List<ReadingOldSession> Items, int TotalCount)> GetReadingOldSessionsWithPagingAsync(string userId, string language, int page, int pageSize);
}
