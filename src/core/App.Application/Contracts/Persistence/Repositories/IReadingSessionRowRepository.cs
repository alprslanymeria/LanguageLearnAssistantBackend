using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR READING SESSION ROW ENTITY.
/// </summary>
public interface IReadingSessionRowRepository
{
    /// <summary>
    /// GETS ALL READING SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<ReadingSessionRow> items, int totalCount)> GetReadingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);

    /// <summary>
    /// CREATES MULTIPLE READING SESSION ROWS.
    /// </summary>
    Task CreateRangeAsync(IEnumerable<ReadingSessionRow> rows);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(ReadingSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ReadingSessionRow?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE READING SESSION ROW IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    ReadingSessionRow Update(ReadingSessionRow entity);

    /// <summary>
    /// REMOVES THE READING SESSION ROW FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(ReadingSessionRow entity);
}
