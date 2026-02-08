using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IWritingSessionRowRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(WritingSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<WritingSessionRow?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE WRITING SESSION ROW IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(WritingSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE WRITING SESSION ROW FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// GETS ALL WRITING SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<WritingSessionRow> Items, int TotalCount)> GetWritingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);

    /// <summary>
    /// CREATES MULTIPLE WRITING SESSION ROWS.
    /// </summary>
    Task AddRangeAsync(IEnumerable<WritingSessionRow> rows);
}
