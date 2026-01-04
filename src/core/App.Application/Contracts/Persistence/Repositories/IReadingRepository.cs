using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR READING ENTITY.
/// </summary>
public interface IReadingRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(Reading entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Reading?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE READING ENTITY IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    Reading Update(Reading entity);

    /// <summary>
    /// REMOVES THE READING ENTITY FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(Reading entity);
}
