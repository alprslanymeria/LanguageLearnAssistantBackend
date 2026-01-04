using App.Domain.Entities;
using App.Domain.Entities.FlashcardEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR PRACTICE ENTITY.
/// </summary>
public interface IPracticeRepository
{
    /// <summary>
    /// GETS PRACTICES BY THE SPECIFIED LANGUAGE.
    /// </summary>
    Task<List<Practice>> GetPracticesByLanguageAsync(string language);

    /// <summary>
    /// CHECKS IF A LANGUAGE EXISTS BY NAME AND LANGUAGE ID.
    /// </summary>
    Task<Practice?> ExistsByLanguageIdAsync(int languageId);

    /// <summary>
    /// CHECKS IF A LANGUAGE EXISTS BY NAME AND LANGUAGE ID.
    /// </summary>
    Task<Practice?> ExistsByNameAndLanguageIdAsync(string name, int languageId);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(Practice entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<Practice?> GetByIdAsync(int id);
        
    /// <summary>
    /// UPDATES THE PRACTICE IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    Practice Update(Practice entity);

    /// <summary>
    /// REMOVES THE PRACTICE FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(Practice entity);
}
