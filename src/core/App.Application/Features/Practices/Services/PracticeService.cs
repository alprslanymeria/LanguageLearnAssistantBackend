using App.Application.Contracts.Infrastructure;
using App.Application.Contracts.Persistence;
using App.Application.Features.Practices.Dtos;
using App.Domain.Entities;
using System.Net;

namespace App.Application.Features.Practices.Services;

public class PracticeService : IPracticeService
{
    private readonly IGenericRepository<Practice, int> _practiceRepository;
    private readonly IGenericRepository<Language, int> _languageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectMapper _objectMapper;

    public PracticeService(
        IGenericRepository<Practice, int> practiceRepository,
        IGenericRepository<Language, int> languageRepository,
        IUnitOfWork unitOfWork,
        IObjectMapper objectMapper)
    {
        _practiceRepository = practiceRepository;
        _languageRepository = languageRepository;
        _unitOfWork = unitOfWork;
        _objectMapper = objectMapper;
    }

    public async Task<ServiceResult<List<PracticeDto>>> GetAllAsync()
    {
        var practices = _practiceRepository.GetAll().ToList();
        var practiceDtos = _objectMapper.Map<List<PracticeDto>>(practices);
        return await Task.FromResult(ServiceResult<List<PracticeDto>>.Success(practiceDtos));
    }

    public async Task<ServiceResult<List<PracticeDto>>> GetByLanguageIdAsync(int languageId)
    {
        var practices = _practiceRepository.Where(p => p.LanguageId == languageId).ToList();
        var practiceDtos = _objectMapper.Map<List<PracticeDto>>(practices);
        return await Task.FromResult(ServiceResult<List<PracticeDto>>.Success(practiceDtos));
    }

    public async Task<ServiceResult<PracticeDto>> GetByIdAsync(int id)
    {
        var practice = await _practiceRepository.GetByIdAsync(id);
        
        if (practice is null)
        {
            return ServiceResult<PracticeDto>.Fail("Practice not found", HttpStatusCode.NotFound);
        }

        var practiceDto = _objectMapper.Map<PracticeDto>(practice);
        return ServiceResult<PracticeDto>.Success(practiceDto);
    }

    public async Task<ServiceResult<PracticeDto>> CreateAsync(CreatePracticeDto dto)
    {
        // Validate language exists
        var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
        if (language is null)
        {
            return ServiceResult<PracticeDto>.Fail("Language not found", HttpStatusCode.BadRequest);
        }

        var practice = _objectMapper.Map<Practice>(dto);
        practice.Language = language;
        
        await _practiceRepository.CreateAsync(practice);
        await _unitOfWork.CommitAsync();

        var practiceDto = _objectMapper.Map<PracticeDto>(practice);
        return ServiceResult<PracticeDto>.SuccessAsCreated(practiceDto, $"/api/practices/{practice.Id}");
    }

    public async Task<ServiceResult<PracticeDto>> UpdateAsync(UpdatePracticeDto dto)
    {
        var existingPractice = await _practiceRepository.GetByIdAsync(dto.Id);
        
        if (existingPractice is null)
        {
            return ServiceResult<PracticeDto>.Fail("Practice not found", HttpStatusCode.NotFound);
        }

        // Validate language exists
        var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
        if (language is null)
        {
            return ServiceResult<PracticeDto>.Fail("Language not found", HttpStatusCode.BadRequest);
        }

        // Update properties manually
        existingPractice.LanguageId = dto.LanguageId;
        existingPractice.Name = dto.Name;
        existingPractice.Language = language;
        
        _practiceRepository.Update(existingPractice);
        await _unitOfWork.CommitAsync();

        var practiceDto = _objectMapper.Map<PracticeDto>(existingPractice);
        return ServiceResult<PracticeDto>.Success(practiceDto);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var practice = await _practiceRepository.GetByIdAsync(id);
        
        if (practice is null)
        {
            return ServiceResult.Fail("Practice not found", HttpStatusCode.NotFound);
        }

        _practiceRepository.Delete(practice);
        await _unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
