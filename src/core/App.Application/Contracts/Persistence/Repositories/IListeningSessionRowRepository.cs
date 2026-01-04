using App.Domain.Entities.ListeningEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR LISTENING SESSION ROW ENTITY.
/// </summary>
public interface IListeningSessionRowRepository
{
    /// <summary>
    /// GETS ALL LISTENING SESSION ROWS FOR A SESSION.
    /// </summary>
    Task<(List<ListeningSessionRow> items, int totalCount)> GetListeningRowsByIdWithPagingAsync(string sessionId, int page, int pageSize);


    /// <summary>
    /// CREATES MULTIPLE LISTENING SESSION ROWS.
    /// </summary>
    Task CreateRangeAsync(IEnumerable<ListeningSessionRow> rows);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(ListeningSessionRow entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ListeningSessionRow?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE LISTENING SESSION ROW IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    ListeningSessionRow Update(ListeningSessionRow entity);

    /// <summary>
    /// REMOVES THE LISTENING SESSION ROW FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(ListeningSessionRow entity);
}
