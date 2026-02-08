using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IWritingOldSessionRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(WritingOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<WritingOldSession?> GetByIdAsync(string id);

    /// <summary>
    /// UPDATES THE WRITING OLD SESSION IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(WritingOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE WRITING OLD SESSION FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(string id);

    /// <summary>
    /// GETS ALL WRITING OLD SESSIONS FOR A USER WITH PAGING AND LANGUAGE FILTER.
    /// </summary>
    Task<(List<WritingOldSession> Items, int TotalCount)> GetWritingOldSessionsWithPagingAsync(string userId, string language, int page, int pageSize);
}
