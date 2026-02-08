using App.Domain.Entities.ListeningEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IListeningSessionRowRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(ListeningSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ListeningSessionRow?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE LISTENING SESSION ROW IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(ListeningSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE LISTENING SESSION ROW FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// GETS ALL LISTENING SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<ListeningSessionRow> Items, int TotalCount)> GetListeningRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);

    /// <summary>
    /// CREATES MULTIPLE LISTENING SESSION ROWS.
    /// </summary>
    Task AddRangeAsync(IEnumerable<ListeningSessionRow> rows);
}
