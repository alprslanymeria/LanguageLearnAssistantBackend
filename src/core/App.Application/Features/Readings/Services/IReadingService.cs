using App.Application.Features.Readings.Dtos;

namespace App.Application.Features.Readings.Services;

public interface IReadingService
{
    Task<ServiceResult<List<ReadingDto>>> GetAllAsync();
    Task<ServiceResult<List<ReadingDto>>> GetByUserIdAsync(string userId);
    Task<ServiceResult<List<ReadingDto>>> GetByPracticeIdAsync(int practiceId);
    Task<ServiceResult<ReadingDto>> GetByIdAsync(int id);
    Task<ServiceResult<ReadingDto>> CreateAsync(CreateReadingDto dto);
    Task<ServiceResult<ReadingDto>> UpdateAsync(UpdateReadingDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
