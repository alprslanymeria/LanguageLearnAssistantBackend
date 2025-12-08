using App.Application.Contracts.Infrastructure;
using App.Application.Contracts.Persistence;
using App.Application.Features.Languages.Dtos;
using App.Domain.Entities;
using System.Net;

namespace App.Application.Features.Languages.Services;

public class LanguageService : ILanguageService
{
    private readonly IGenericRepository<Language, int> _languageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectMapper _objectMapper;

    public LanguageService(
        IGenericRepository<Language, int> languageRepository,
        IUnitOfWork unitOfWork,
        IObjectMapper objectMapper)
    {
        _languageRepository = languageRepository;
        _unitOfWork = unitOfWork;
        _objectMapper = objectMapper;
    }

    public async Task<ServiceResult<List<LanguageDto>>> GetAllAsync()
    {
        var languages = _languageRepository.GetAll().ToList();
        var languageDtos = _objectMapper.Map<List<LanguageDto>>(languages);
        return await Task.FromResult(ServiceResult<List<LanguageDto>>.Success(languageDtos));
    }

    public async Task<ServiceResult<LanguageDto>> GetByIdAsync(int id)
    {
        var language = await _languageRepository.GetByIdAsync(id);
        
        if (language is null)
        {
            return ServiceResult<LanguageDto>.Fail("Language not found", HttpStatusCode.NotFound);
        }

        var languageDto = _objectMapper.Map<LanguageDto>(language);
        return ServiceResult<LanguageDto>.Success(languageDto);
    }

    public async Task<ServiceResult<LanguageDto>> CreateAsync(CreateLanguageDto dto)
    {
        var language = _objectMapper.Map<Language>(dto);
        await _languageRepository.CreateAsync(language);
        await _unitOfWork.CommitAsync();

        var languageDto = _objectMapper.Map<LanguageDto>(language);
        return ServiceResult<LanguageDto>.SuccessAsCreated(languageDto, $"/api/languages/{language.Id}");
    }

    public async Task<ServiceResult<LanguageDto>> UpdateAsync(UpdateLanguageDto dto)
    {
        var existingLanguage = await _languageRepository.GetByIdAsync(dto.Id);
        
        if (existingLanguage is null)
        {
            return ServiceResult<LanguageDto>.Fail("Language not found", HttpStatusCode.NotFound);
        }

        // Update properties manually
        existingLanguage.Name = dto.Name;
        existingLanguage.ImageUrl = dto.ImageUrl;
        
        _languageRepository.Update(existingLanguage);
        await _unitOfWork.CommitAsync();

        var languageDto = _objectMapper.Map<LanguageDto>(existingLanguage);
        return ServiceResult<LanguageDto>.Success(languageDto);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var language = await _languageRepository.GetByIdAsync(id);
        
        if (language is null)
        {
            return ServiceResult.Fail("Language not found", HttpStatusCode.NotFound);
        }

        _languageRepository.Delete(language);
        await _unitOfWork.CommitAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }
}
