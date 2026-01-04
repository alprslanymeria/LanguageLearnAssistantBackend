using App.Domain.Entities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR LANGUAGE ENTITY.
/// </summary>
public interface ILanguageRepository
{
    /// <summary>
    /// GETS ALL LANGUAGES WITHOUT PRACTICES.
    /// </summary>
    Task<List<Language>> GetLanguagesAsync();

    /// <summary>
    /// CHECKS IF A LANGUAGE EXISTS BY NAME.
    /// </summary>
    Task<Language?> ExistsByNameAsync(string name);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(Language entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Language?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE LANGUAGE IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    Language Update(Language entity);

    /// <summary>
    /// REMOVES THE LANGUAGE FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(Language entity);
}
