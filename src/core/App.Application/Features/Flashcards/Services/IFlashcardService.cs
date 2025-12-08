using App.Application.Features.Flashcards.Dtos;

namespace App.Application.Features.Flashcards.Services;

public interface IFlashcardService
{
    Task<ServiceResult<List<FlashcardDto>>> GetAllAsync();
    Task<ServiceResult<List<FlashcardDto>>> GetByUserIdAsync(string userId);
    Task<ServiceResult<List<FlashcardDto>>> GetByPracticeIdAsync(int practiceId);
    Task<ServiceResult<FlashcardDto>> GetByIdAsync(int id);
    Task<ServiceResult<FlashcardDto>> CreateAsync(CreateFlashcardDto dto);
    Task<ServiceResult<FlashcardDto>> UpdateAsync(UpdateFlashcardDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
