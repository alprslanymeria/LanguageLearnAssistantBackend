using App.Application.Common;
using App.Application.Features.Practices.Dtos;

namespace App.Application.Features.Practices;

/// <summary>
/// SERVICE INTERFACE FOR PRACTICE OPERATIONS.
/// </summary>
public interface IPracticeService
{
    /// <summary>
    /// RETRIEVES PRACTICES BY SPECIFIED LANGUAGE.
    /// </summary>
    Task<ServiceResult<List<PracticeDto>>> GetPracticesByLanguageAsync(string language);
}
