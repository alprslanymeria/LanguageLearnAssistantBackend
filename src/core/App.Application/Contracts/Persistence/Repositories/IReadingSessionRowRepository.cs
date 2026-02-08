using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IReadingSessionRowRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(ReadingSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ReadingSessionRow?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE READING SESSION ROW IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(ReadingSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE READING SESSION ROW FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// GETS ALL READING SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<ReadingSessionRow> Items, int TotalCount)> GetReadingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);

    /// <summary>
    /// CREATES MULTIPLE READING SESSION ROWS.
    /// </summary>
    Task AddRangeAsync(IEnumerable<ReadingSessionRow> rows);
}
