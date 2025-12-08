using App.Application;
using App.Application.Features.Listenings.Dtos;

namespace App.Application.Features.Listenings.Services;

public interface IListeningService
{
    Task<ServiceResult<List<ListeningDto>>> GetAllAsync();
    Task<ServiceResult<List<ListeningDto>>> GetByUserIdAsync(string userId);
    Task<ServiceResult<List<ListeningDto>>> GetByPracticeIdAsync(int practiceId);
    Task<ServiceResult<ListeningDto>> GetByIdAsync(int id);
    Task<ServiceResult<ListeningDto>> CreateAsync(CreateListeningDto dto);
    Task<ServiceResult<ListeningDto>> UpdateAsync(UpdateListeningDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
