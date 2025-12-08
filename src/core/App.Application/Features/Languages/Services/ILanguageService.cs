using App.Application.Features.Languages.Dtos;

namespace App.Application.Features.Languages.Services;

public interface ILanguageService
{
    Task<ServiceResult<List<LanguageDto>>> GetAllAsync();
    Task<ServiceResult<LanguageDto>> GetByIdAsync(int id);
    Task<ServiceResult<LanguageDto>> CreateAsync(CreateLanguageDto dto);
    Task<ServiceResult<LanguageDto>> UpdateAsync(UpdateLanguageDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
