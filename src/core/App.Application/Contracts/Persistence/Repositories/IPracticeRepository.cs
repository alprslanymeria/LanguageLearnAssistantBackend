using App.Domain.Entities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR PRACTICE ENTITY.
/// </summary>
public interface IPracticeRepository : IGenericRepository<Practice, int>
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
}
