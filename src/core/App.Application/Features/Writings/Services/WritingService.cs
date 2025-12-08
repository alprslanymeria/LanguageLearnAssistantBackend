using App.Application.Contracts.Infrastructure;
using App.Application.Contracts.Persistence;
using App.Application.Features.Writings.Dtos;
using App.Domain.Entities;
using App.Domain.Entities.WritingEntities;
using System.Net;

namespace App.Application.Features.Writings.Services;

public class WritingService : IWritingService
{
    private readonly IGenericRepository<Writing, int> _writingRepository;
    private readonly IGenericRepository<Language, int> _languageRepository;
    private readonly IGenericRepository<Practice, int> _practiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectMapper _objectMapper;

    public WritingService(
        IGenericRepository<Writing, int> writingRepository,
        IGenericRepository<Language, int> languageRepository,
        IGenericRepository<Practice, int> practiceRepository,
        IUnitOfWork unitOfWork,
        IObjectMapper objectMapper)
    {
        _writingRepository = writingRepository;
        _languageRepository = languageRepository;
        _practiceRepository = practiceRepository;
        _unitOfWork = unitOfWork;
        _objectMapper = objectMapper;
    }

    public async Task<ServiceResult<List<WritingDto>>> GetAllAsync()
    {
        var writings = _writingRepository.GetAll().ToList();
        var writingDtos = _objectMapper.Map<List<WritingDto>>(writings);
        return ServiceResult<List<WritingDto>>.Success(writingDtos);
    }

    public async Task<ServiceResult<List<WritingDto>>> GetByUserIdAsync(string userId)
    {
        var writings = _writingRepository.Where(x => x.UserId == userId).ToList();
        var writingDtos = _objectMapper.Map<List<WritingDto>>(writings);
        return ServiceResult<List<WritingDto>>.Success(writingDtos);
    }

    public async Task<ServiceResult<List<WritingDto>>> GetByPracticeIdAsync(int practiceId)
    {
        var writings = _writingRepository.Where(x => x.PracticeId == practiceId).ToList();
        var writingDtos = _objectMapper.Map<List<WritingDto>>(writings);
        return ServiceResult<List<WritingDto>>.Success(writingDtos);
    }

    public async Task<ServiceResult<WritingDto>> GetByIdAsync(int id)
    {
        var writing = await _writingRepository.GetByIdAsync(id);
        
        if (writing is null)
        {
            return ServiceResult<WritingDto>.Fail("Writing not found", HttpStatusCode.NotFound);
        }

        var writingDto = _objectMapper.Map<WritingDto>(writing);
        return ServiceResult<WritingDto>.Success(writingDto);
    }

    public async Task<ServiceResult<WritingDto>> CreateAsync(CreateWritingDto dto)
    {
        // Validate language exists
        var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
        if (language is null)
        {
            return ServiceResult<WritingDto>.Fail("Language not found", HttpStatusCode.BadRequest);
        }

        // Validate practice exists
        var practice = await _practiceRepository.GetByIdAsync(dto.PracticeId);
        if (practice is null)
        {
            return ServiceResult<WritingDto>.Fail("Practice not found", HttpStatusCode.BadRequest);
        }

        var writing = _objectMapper.Map<Writing>(dto);
        writing.Language = language;
        writing.Practice = practice;
        
        await _writingRepository.CreateAsync(writing);
        await _unitOfWork.CommitAsync();

        var writingDto = _objectMapper.Map<WritingDto>(writing);
        return ServiceResult<WritingDto>.SuccessAsCreated(writingDto, $"/api/writings/{writing.Id}");
    }

    public async Task<ServiceResult<WritingDto>> UpdateAsync(UpdateWritingDto dto)
    {
        var existingWriting = await _writingRepository.GetByIdAsync(dto.Id);
        
        if (existingWriting is null)
        {
            return ServiceResult<WritingDto>.Fail("Writing not found", HttpStatusCode.NotFound);
        }

        // Validate language exists
        var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
        if (language is null)
        {
            return ServiceResult<WritingDto>.Fail("Language not found", HttpStatusCode.BadRequest);
        }

        // Validate practice exists
        var practice = await _practiceRepository.GetByIdAsync(dto.PracticeId);
        if (practice is null)
        {
            return ServiceResult<WritingDto>.Fail("Practice not found", HttpStatusCode.BadRequest);
        }

        // Update properties manually
        existingWriting.UserId = dto.UserId;
        existingWriting.LanguageId = dto.LanguageId;
        existingWriting.PracticeId = dto.PracticeId;
        existingWriting.Language = language;
        existingWriting.Practice = practice;
        
        _writingRepository.Update(existingWriting);
        await _unitOfWork.CommitAsync();

        var writingDto = _objectMapper.Map<WritingDto>(existingWriting);
        return ServiceResult<WritingDto>.Success(writingDto);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var writing = await _writingRepository.GetByIdAsync(id);
        
        if (writing is null)
        {
            return ServiceResult.Fail("Writing not found", HttpStatusCode.NotFound);
        }

        _writingRepository.Delete(writing);
        await _unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
