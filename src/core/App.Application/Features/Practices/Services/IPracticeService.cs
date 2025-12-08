using App.Application;
using App.Application.Features.Practices.Dtos;

namespace App.Application.Features.Practices.Services;

public interface IPracticeService
{
    Task<ServiceResult<List<PracticeDto>>> GetAllAsync();
    Task<ServiceResult<List<PracticeDto>>> GetByLanguageIdAsync(int languageId);
    Task<ServiceResult<PracticeDto>> GetByIdAsync(int id);
    Task<ServiceResult<PracticeDto>> CreateAsync(CreatePracticeDto dto);
    Task<ServiceResult<PracticeDto>> UpdateAsync(UpdatePracticeDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
