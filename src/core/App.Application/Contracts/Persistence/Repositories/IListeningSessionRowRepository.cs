using App.Domain.Entities.ListeningEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR LISTENING SESSION ROW ENTITY.
/// </summary>
public interface IListeningSessionRowRepository : IGenericRepository<ListeningSessionRow, int>
{
    /// <summary>
    /// GETS ALL LISTENING SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<ListeningSessionRow> items, int totalCount)> GetListeningRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);


    /// <summary>
    /// CREATES MULTIPLE LISTENING SESSION ROWS.
    /// </summary>
    Task CreateRangeAsync(IEnumerable<ListeningSessionRow> rows);
}
