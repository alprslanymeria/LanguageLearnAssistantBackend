using App.Domain.Entities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface ILanguageRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(Language entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Language?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE LANGUAGE IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(Language entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE LANGUAGE FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// GETS ALL LANGUAGES.
    /// </summary>
    Task<List<Language>> GetLanguagesAsync();

    /// <summary>
    /// CHECKS IF A LANGUAGE EXISTS BY NAME.
    /// </summary>
    Task<Language?> ExistsByNameAsync(string name);

    /// <summary>
    /// GETS A LANGUAGE BY NAME.
    /// </summary>
    Task<Language?> GetByNameAsync(string name);

    /// <summary>
    /// GETS ALL LANGUAGES.
    /// </summary>
    Task<List<Language>> GetAllAsync();
}
