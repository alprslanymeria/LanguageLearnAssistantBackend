using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR WRITING ENTITY.
/// </summary>
public interface IWritingRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(Writing entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Writing?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE WRITING ENTITY IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    Writing Update(Writing entity);

    /// <summary>
    /// REMOVES THE WRITING ENTITY FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(Writing entity);
}
