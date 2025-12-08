using App.Application.Contracts.Infrastructure;
using App.Application.Contracts.Persistence;
using App.Application.Features.Flashcards.Dtos;
using App.Domain.Entities;
using App.Domain.Entities.FlashcardEntities;
using System.Net;

namespace App.Application.Features.Flashcards.Services;

public class FlashcardService : IFlashcardService
{
    private readonly IGenericRepository<Flashcard, int> _flashcardRepository;
    private readonly IGenericRepository<Language, int> _languageRepository;
    private readonly IGenericRepository<Practice, int> _practiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectMapper _objectMapper;

    public FlashcardService(
        IGenericRepository<Flashcard, int> flashcardRepository,
        IGenericRepository<Language, int> languageRepository,
        IGenericRepository<Practice, int> practiceRepository,
        IUnitOfWork unitOfWork,
        IObjectMapper objectMapper)
    {
        _flashcardRepository = flashcardRepository;
        _languageRepository = languageRepository;
        _practiceRepository = practiceRepository;
        _unitOfWork = unitOfWork;
        _objectMapper = objectMapper;
    }

    public async Task<ServiceResult<List<FlashcardDto>>> GetAllAsync()
    {
        var flashcards = _flashcardRepository.GetAll().ToList();
        var flashcardDtos = _objectMapper.Map<List<FlashcardDto>>(flashcards);
        return ServiceResult<List<FlashcardDto>>.Success(flashcardDtos);
    }

    public async Task<ServiceResult<List<FlashcardDto>>> GetByUserIdAsync(string userId)
    {
        var flashcards = _flashcardRepository.Where(x => x.UserId == userId).ToList();
        var flashcardDtos = _objectMapper.Map<List<FlashcardDto>>(flashcards);
        return ServiceResult<List<FlashcardDto>>.Success(flashcardDtos);
    }

    public async Task<ServiceResult<List<FlashcardDto>>> GetByPracticeIdAsync(int practiceId)
    {
        var flashcards = _flashcardRepository.Where(x => x.PracticeId == practiceId).ToList();
        var flashcardDtos = _objectMapper.Map<List<FlashcardDto>>(flashcards);
        return ServiceResult<List<FlashcardDto>>.Success(flashcardDtos);
    }

    public async Task<ServiceResult<FlashcardDto>> GetByIdAsync(int id)
    {
        var flashcard = await _flashcardRepository.GetByIdAsync(id);
        
        if (flashcard is null)
        {
            return ServiceResult<FlashcardDto>.Fail("Flashcard not found", HttpStatusCode.NotFound);
        }

        var flashcardDto = _objectMapper.Map<FlashcardDto>(flashcard);
        return ServiceResult<FlashcardDto>.Success(flashcardDto);
    }

    public async Task<ServiceResult<FlashcardDto>> CreateAsync(CreateFlashcardDto dto)
    {
        // Validate language exists
        var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
        if (language is null)
        {
            return ServiceResult<FlashcardDto>.Fail("Language not found", HttpStatusCode.BadRequest);
        }

        // Validate practice exists
        var practice = await _practiceRepository.GetByIdAsync(dto.PracticeId);
        if (practice is null)
        {
            return ServiceResult<FlashcardDto>.Fail("Practice not found", HttpStatusCode.BadRequest);
        }

        var flashcard = _objectMapper.Map<Flashcard>(dto);
        flashcard.Language = language;
        flashcard.Practice = practice;
        
        await _flashcardRepository.CreateAsync(flashcard);
        await _unitOfWork.CommitAsync();

        var flashcardDto = _objectMapper.Map<FlashcardDto>(flashcard);
        return ServiceResult<FlashcardDto>.SuccessAsCreated(flashcardDto, $"/api/flashcards/{flashcard.Id}");
    }

    public async Task<ServiceResult<FlashcardDto>> UpdateAsync(UpdateFlashcardDto dto)
    {
        var existingFlashcard = await _flashcardRepository.GetByIdAsync(dto.Id);
        
        if (existingFlashcard is null)
        {
            return ServiceResult<FlashcardDto>.Fail("Flashcard not found", HttpStatusCode.NotFound);
        }

        // Validate language exists
        var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
        if (language is null)
        {
            return ServiceResult<FlashcardDto>.Fail("Language not found", HttpStatusCode.BadRequest);
        }

        // Validate practice exists
        var practice = await _practiceRepository.GetByIdAsync(dto.PracticeId);
        if (practice is null)
        {
            return ServiceResult<FlashcardDto>.Fail("Practice not found", HttpStatusCode.BadRequest);
        }

        // Update properties manually
        existingFlashcard.UserId = dto.UserId;
        existingFlashcard.LanguageId = dto.LanguageId;
        existingFlashcard.PracticeId = dto.PracticeId;
        existingFlashcard.Language = language;
        existingFlashcard.Practice = practice;
        
        _flashcardRepository.Update(existingFlashcard);
        await _unitOfWork.CommitAsync();

        var flashcardDto = _objectMapper.Map<FlashcardDto>(existingFlashcard);
        return ServiceResult<FlashcardDto>.Success(flashcardDto);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var flashcard = await _flashcardRepository.GetByIdAsync(id);
        
        if (flashcard is null)
        {
            return ServiceResult.Fail("Flashcard not found", HttpStatusCode.NotFound);
        }

        _flashcardRepository.Delete(flashcard);
        await _unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
