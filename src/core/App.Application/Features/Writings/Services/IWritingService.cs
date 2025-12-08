using App.Application;
using App.Application.Features.Writings.Dtos;

namespace App.Application.Features.Writings.Services;

public interface IWritingService
{
    Task<ServiceResult<List<WritingDto>>> GetAllAsync();
    Task<ServiceResult<List<WritingDto>>> GetByUserIdAsync(string userId);
    Task<ServiceResult<List<WritingDto>>> GetByPracticeIdAsync(int practiceId);
    Task<ServiceResult<WritingDto>> GetByIdAsync(int id);
    Task<ServiceResult<WritingDto>> CreateAsync(CreateWritingDto dto);
    Task<ServiceResult<WritingDto>> UpdateAsync(UpdateWritingDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
