using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IReadingRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(Reading entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Reading?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE READING ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(Reading entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE READING ENTITY FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES A READING BY ITS PRACTICE ID, USER ID, AND LANGUAGE ID.
    /// </summary>
    Task<Reading?> GetByPracticeIdUserIdLanguageIdAsync(int practiceId, string userId, int languageId);
}
