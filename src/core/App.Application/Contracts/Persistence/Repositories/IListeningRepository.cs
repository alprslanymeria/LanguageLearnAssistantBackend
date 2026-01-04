using App.Domain.Entities.ListeningEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR LISTENING ENTITY.
/// </summary>
public interface IListeningRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(Listening entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Listening?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE LISTENING ENTITY IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    Listening Update(Listening entity);

    /// <summary>
    /// REMOVES THE LISTENING ENTITY FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(Listening entity);
}
