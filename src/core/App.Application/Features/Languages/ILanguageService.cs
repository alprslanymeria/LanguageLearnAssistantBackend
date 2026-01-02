using App.Application.Common;
using App.Application.Features.Languages.Dtos;

namespace App.Application.Features.Languages;

/// <summary>
/// SERVICE INTERFACE FOR LANGUAGE OPERATIONS.
/// </summary>
public interface ILanguageService
{
    /// <summary>
    /// RETRIEVES ALL LANGUAGES WITH PRACTICE COUNTS.
    /// </summary>
    Task<ServiceResult<List<LanguageDto>>> GetLanguagesAsync();
}
