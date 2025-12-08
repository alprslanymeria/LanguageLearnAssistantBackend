using App.Application.Features.Languages.DTOs;

namespace App.Application.Features.Languages.Services;

public interface ILanguageService
{
    Task<ServiceResult<List<LanguageDto>>> GetLanguagesAsync();
    Task<ServiceResult<CompareLanguageIdResponse>> CompareLanguageIdAsync(CompareLanguageIdRequest request);
}
