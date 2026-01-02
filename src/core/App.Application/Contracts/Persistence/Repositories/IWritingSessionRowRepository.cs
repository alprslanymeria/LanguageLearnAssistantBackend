using App.Domain.Entities.ReadingEntities;
using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR WRITING SESSION ROW ENTITY.
/// </summary>
public interface IWritingSessionRowRepository : IGenericRepository<WritingSessionRow, int>
{
    /// <summary>
    /// GETS ALL WRITING SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<WritingSessionRow> items, int totalCount)> GetWritingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);

    /// <summary>
    /// CREATES MULTIPLE WRITING SESSION ROWS.
    /// </summary>
    Task CreateRangeAsync(IEnumerable<WritingSessionRow> rows);
}
