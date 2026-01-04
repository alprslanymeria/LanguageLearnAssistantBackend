using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR READING OLD SESSION ENTITY.
/// </summary>
public interface IReadingOldSessionRepository
{
    /// <summary>
    /// GETS ALL READING OLD SESSIONS FOR A USER WITH DETAILS.
    /// </summary>
    Task<(List<ReadingOldSession> items, int totalCount)> GetReadingOldSessionsWithPagingAsync(string userId, int page, int pageSize);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(ReadingOldSession entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ReadingOldSession?> GetByIdAsync(string id);

    /// <summary>
    /// UPDATES THE READING OLD SESSION IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    ReadingOldSession Update(ReadingOldSession entity);

    /// <summary>
    /// REMOVES THE READING OLD SESSION FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(ReadingOldSession entity);
}
