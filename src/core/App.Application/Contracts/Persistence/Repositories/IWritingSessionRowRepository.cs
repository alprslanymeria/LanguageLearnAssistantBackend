using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR WRITING SESSION ROW ENTITY.
/// </summary>
public interface IWritingSessionRowRepository
{
    /// <summary>
    /// GETS ALL WRITING SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<WritingSessionRow> items, int totalCount)> GetWritingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);

    /// <summary>
    /// CREATES MULTIPLE WRITING SESSION ROWS.
    /// </summary>
    Task CreateRangeAsync(IEnumerable<WritingSessionRow> rows);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(WritingSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<WritingSessionRow?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE WRITING SESSION ROW IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    WritingSessionRow Update(WritingSessionRow entity);

    /// <summary>
    /// REMOVES THE WRITING SESSION ROW FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(WritingSessionRow entity);
}
