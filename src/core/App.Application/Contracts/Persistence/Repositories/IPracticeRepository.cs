using App.Domain.Entities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IPracticeRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(Practice entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Practice?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE PRACTICE IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(Practice entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE PRACTICE FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// GETS PRACTICES BY THE SPECIFIED LANGUAGE NAME.
    /// </summary>
    Task<List<Practice>> GetPracticesByLanguageAsync(string language);

    /// <summary>
    /// GETS A PRACTICE BY LANGUAGE ID AND NAME.
    /// </summary>
    Task<Practice?> GetPracticeByLanguageIdAndNameAsync(int languageId, string name);

    /// <summary>
    /// CHECKS IF A PRACTICE EXISTS BY LANGUAGE ID.
    /// </summary>
    Task<Practice?> ExistsByLanguageIdAsync(int languageId);

    /// <summary>
    /// CHECKS IF A PRACTICE EXISTS BY NAME AND LANGUAGE ID.
    /// </summary>
    Task<Practice?> ExistsByNameAndLanguageIdAsync(string name, int languageId);
}
