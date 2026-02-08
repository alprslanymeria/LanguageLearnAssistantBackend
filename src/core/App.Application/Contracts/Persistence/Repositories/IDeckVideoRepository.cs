using App.Domain.Entities.ListeningEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IDeckVideoRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(DeckVideo entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<DeckVideo?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE DECK VIDEO IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(DeckVideo entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE DECK VIDEO FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);
}
