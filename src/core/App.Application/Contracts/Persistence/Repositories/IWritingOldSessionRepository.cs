using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR WRITING OLD SESSION ENTITY.
/// </summary>
public interface IWritingOldSessionRepository
{
    /// <summary>
    /// GETS ALL WRITING OLD SESSIONS FOR A USER WITH DETAILS.
    /// </summary>
    Task<(List<WritingOldSession> items, int totalCount)> GetWritingOldSessionsWithPagingAsync(string userId, int page, int pageSize);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(WritingOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<WritingOldSession?> GetByIdAsync(string id);

    /// <summary>
    /// UPDATES THE WRITING OLD SESSION IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    WritingOldSession Update(WritingOldSession entity);

    /// <summary>
    /// REMOVES THE WRITING OLD SESSION FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(WritingOldSession entity);
}
