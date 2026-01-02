using App.Domain.Entities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR LANGUAGE ENTITY.
/// </summary>
public interface ILanguageRepository : IGenericRepository<Language, int>
{
    /// <summary>
    /// GETS ALL LANGUAGES WITHOUT PRACTICES.
    /// </summary>
    Task<List<Language>> GetLanguagesAsync();

    /// <summary>
    /// CHECKS IF A LANGUAGE EXISTS BY NAME.
    /// </summary>
    Task<Language?> ExistsByNameAsync(string name);
}
