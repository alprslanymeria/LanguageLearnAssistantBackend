using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR READING SESSION ROW ENTITY.
/// </summary>
public interface IReadingSessionRowRepository : IGenericRepository<ReadingSessionRow, int>
{
    /// <summary>
    /// GETS ALL READING SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<ReadingSessionRow> items, int totalCount)> GetReadingRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);

    /// <summary>
    /// CREATES MULTIPLE READING SESSION ROWS.
    /// </summary>
    Task CreateRangeAsync(IEnumerable<ReadingSessionRow> rows);
}
