using App.Application.Contracts.Infrastructure;
using App.Application.Contracts.Persistence;
using App.Application.Features.Readings.Dtos;
using App.Domain.Entities;
using App.Domain.Entities.ReadingEntities;
using System.Net;

namespace App.Application.Features.Readings.Services;

public class ReadingService : IReadingService
{
    private readonly IGenericRepository<Reading, int> _readingRepository;
    private readonly IGenericRepository<Language, int> _languageRepository;
    private readonly IGenericRepository<Practice, int> _practiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectMapper _objectMapper;

    public ReadingService(
        IGenericRepository<Reading, int> readingRepository,
        IGenericRepository<Language, int> languageRepository,
        IGenericRepository<Practice, int> practiceRepository,
        IUnitOfWork unitOfWork,
        IObjectMapper objectMapper)
    {
        _readingRepository = readingRepository;
        _languageRepository = languageRepository;
        _practiceRepository = practiceRepository;
        _unitOfWork = unitOfWork;
        _objectMapper = objectMapper;
    }

    public async Task<ServiceResult<List<ReadingDto>>> GetAllAsync()
    {
        var readings = _readingRepository.GetAll().ToList();
        var readingDtos = _objectMapper.Map<List<ReadingDto>>(readings);
        return ServiceResult<List<ReadingDto>>.Success(readingDtos);
    }

    public async Task<ServiceResult<List<ReadingDto>>> GetByUserIdAsync(string userId)
    {
        var readings = _readingRepository.Where(x => x.UserId == userId).ToList();
        var readingDtos = _objectMapper.Map<List<ReadingDto>>(readings);
        return ServiceResult<List<ReadingDto>>.Success(readingDtos);
    }

    public async Task<ServiceResult<List<ReadingDto>>> GetByPracticeIdAsync(int practiceId)
    {
        var readings = _readingRepository.Where(x => x.PracticeId == practiceId).ToList();
        var readingDtos = _objectMapper.Map<List<ReadingDto>>(readings);
        return ServiceResult<List<ReadingDto>>.Success(readingDtos);
    }

    public async Task<ServiceResult<ReadingDto>> GetByIdAsync(int id)
    {
        var reading = await _readingRepository.GetByIdAsync(id);
        
        if (reading is null)
        {
            return ServiceResult<ReadingDto>.Fail("Reading not found", HttpStatusCode.NotFound);
        }

        var readingDto = _objectMapper.Map<ReadingDto>(reading);
        return ServiceResult<ReadingDto>.Success(readingDto);
    }

    public async Task<ServiceResult<ReadingDto>> CreateAsync(CreateReadingDto dto)
    {
        // Validate language exists
        var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
        if (language is null)
        {
            return ServiceResult<ReadingDto>.Fail("Language not found", HttpStatusCode.BadRequest);
        }

        // Validate practice exists
        var practice = await _practiceRepository.GetByIdAsync(dto.PracticeId);
        if (practice is null)
        {
            return ServiceResult<ReadingDto>.Fail("Practice not found", HttpStatusCode.BadRequest);
        }

        var reading = _objectMapper.Map<Reading>(dto);
        reading.Language = language;
        reading.Practice = practice;
        
        await _readingRepository.CreateAsync(reading);
        await _unitOfWork.CommitAsync();

        var readingDto = _objectMapper.Map<ReadingDto>(reading);
        return ServiceResult<ReadingDto>.SuccessAsCreated(readingDto, $"/api/readings/{reading.Id}");
    }

    public async Task<ServiceResult<ReadingDto>> UpdateAsync(UpdateReadingDto dto)
    {
        var existingReading = await _readingRepository.GetByIdAsync(dto.Id);
        
        if (existingReading is null)
        {
            return ServiceResult<ReadingDto>.Fail("Reading not found", HttpStatusCode.NotFound);
        }

        // Validate language exists
        var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
        if (language is null)
        {
            return ServiceResult<ReadingDto>.Fail("Language not found", HttpStatusCode.BadRequest);
        }

        // Validate practice exists
        var practice = await _practiceRepository.GetByIdAsync(dto.PracticeId);
        if (practice is null)
        {
            return ServiceResult<ReadingDto>.Fail("Practice not found", HttpStatusCode.BadRequest);
        }

        // Update properties manually
        existingReading.UserId = dto.UserId;
        existingReading.LanguageId = dto.LanguageId;
        existingReading.PracticeId = dto.PracticeId;
        existingReading.Language = language;
        existingReading.Practice = practice;
        
        _readingRepository.Update(existingReading);
        await _unitOfWork.CommitAsync();

        var readingDto = _objectMapper.Map<ReadingDto>(existingReading);
        return ServiceResult<ReadingDto>.Success(readingDto);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var reading = await _readingRepository.GetByIdAsync(id);
        
        if (reading is null)
        {
            return ServiceResult.Fail("Reading not found", HttpStatusCode.NotFound);
        }

        _readingRepository.Delete(reading);
        await _unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
