using App.Domain.Entities.ListeningEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IListeningRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(Listening entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Listening?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE LISTENING ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(Listening entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE LISTENING ENTITY FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES A LISTENING BY ITS PRACTICE ID, USER ID, AND LANGUAGE ID.
    /// </summary>
    Task<Listening?> GetByPracticeIdUserIdLanguageIdAsync(int practiceId, string userId, int languageId);
}
