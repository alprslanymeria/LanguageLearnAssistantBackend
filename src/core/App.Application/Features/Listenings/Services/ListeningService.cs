using App.Application.Contracts.Infrastructure;
using App.Application.Contracts.Persistence;
using App.Application.Features.Listenings.Dtos;
using App.Domain.Entities;
using App.Domain.Entities.ListeningEntities;
using System.Net;

namespace App.Application.Features.Listenings.Services;

public class ListeningService : IListeningService
{
    private readonly IGenericRepository<Listening, int> _listeningRepository;
    private readonly IGenericRepository<Language, int> _languageRepository;
    private readonly IGenericRepository<Practice, int> _practiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectMapper _objectMapper;

    public ListeningService(
        IGenericRepository<Listening, int> listeningRepository,
        IGenericRepository<Language, int> languageRepository,
        IGenericRepository<Practice, int> practiceRepository,
        IUnitOfWork unitOfWork,
        IObjectMapper objectMapper)
    {
        _listeningRepository = listeningRepository;
        _languageRepository = languageRepository;
        _practiceRepository = practiceRepository;
        _unitOfWork = unitOfWork;
        _objectMapper = objectMapper;
    }

    public async Task<ServiceResult<List<ListeningDto>>> GetAllAsync()
    {
        var listenings = _listeningRepository.GetAll().ToList();
        var listeningDtos = _objectMapper.Map<List<ListeningDto>>(listenings);
        return await Task.FromResult(ServiceResult<List<ListeningDto>>.Success(listeningDtos));
    }

    public async Task<ServiceResult<List<ListeningDto>>> GetByUserIdAsync(string userId)
    {
        var listenings = _listeningRepository.Where(x => x.UserId == userId).ToList();
        var listeningDtos = _objectMapper.Map<List<ListeningDto>>(listenings);
        return await Task.FromResult(ServiceResult<List<ListeningDto>>.Success(listeningDtos));
    }

    public async Task<ServiceResult<List<ListeningDto>>> GetByPracticeIdAsync(int practiceId)
    {
        var listenings = _listeningRepository.Where(x => x.PracticeId == practiceId).ToList();
        var listeningDtos = _objectMapper.Map<List<ListeningDto>>(listenings);
        return await Task.FromResult(ServiceResult<List<ListeningDto>>.Success(listeningDtos));
    }

    public async Task<ServiceResult<ListeningDto>> GetByIdAsync(int id)
    {
        var listening = await _listeningRepository.GetByIdAsync(id);
        
        if (listening is null)
        {
            return ServiceResult<ListeningDto>.Fail("Listening not found", HttpStatusCode.NotFound);
        }

        var listeningDto = _objectMapper.Map<ListeningDto>(listening);
        return ServiceResult<ListeningDto>.Success(listeningDto);
    }

    public async Task<ServiceResult<ListeningDto>> CreateAsync(CreateListeningDto dto)
    {
        // Validate language exists
        var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
        if (language is null)
        {
            return ServiceResult<ListeningDto>.Fail("Language not found", HttpStatusCode.BadRequest);
        }

        // Validate practice exists
        var practice = await _practiceRepository.GetByIdAsync(dto.PracticeId);
        if (practice is null)
        {
            return ServiceResult<ListeningDto>.Fail("Practice not found", HttpStatusCode.BadRequest);
        }

        var listening = _objectMapper.Map<Listening>(dto);
        listening.Language = language;
        listening.Practice = practice;
        
        await _listeningRepository.CreateAsync(listening);
        await _unitOfWork.CommitAsync();

        var listeningDto = _objectMapper.Map<ListeningDto>(listening);
        return ServiceResult<ListeningDto>.SuccessAsCreated(listeningDto, $"/api/listenings/{listening.Id}");
    }

    public async Task<ServiceResult<ListeningDto>> UpdateAsync(UpdateListeningDto dto)
    {
        var existingListening = await _listeningRepository.GetByIdAsync(dto.Id);
        
        if (existingListening is null)
        {
            return ServiceResult<ListeningDto>.Fail("Listening not found", HttpStatusCode.NotFound);
        }

        // Validate language exists
        var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
        if (language is null)
        {
            return ServiceResult<ListeningDto>.Fail("Language not found", HttpStatusCode.BadRequest);
        }

        // Validate practice exists
        var practice = await _practiceRepository.GetByIdAsync(dto.PracticeId);
        if (practice is null)
        {
            return ServiceResult<ListeningDto>.Fail("Practice not found", HttpStatusCode.BadRequest);
        }

        // Update properties manually
        existingListening.UserId = dto.UserId;
        existingListening.LanguageId = dto.LanguageId;
        existingListening.PracticeId = dto.PracticeId;
        existingListening.Language = language;
        existingListening.Practice = practice;
        
        _listeningRepository.Update(existingListening);
        await _unitOfWork.CommitAsync();

        var listeningDto = _objectMapper.Map<ListeningDto>(existingListening);
        return ServiceResult<ListeningDto>.Success(listeningDto);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var listening = await _listeningRepository.GetByIdAsync(id);
        
        if (listening is null)
        {
            return ServiceResult.Fail("Listening not found", HttpStatusCode.NotFound);
        }

        _listeningRepository.Delete(listening);
        await _unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
