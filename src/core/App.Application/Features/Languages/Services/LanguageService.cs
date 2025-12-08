using System.Net;
using App.Application.Contracts.Persistence;
using App.Application.Features.Languages.DTOs;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.Languages.Services;

public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _languageRepository;
    private readonly ILogger<LanguageService> _logger;

    public LanguageService(ILanguageRepository languageRepository, ILogger<LanguageService> logger)
    {
        _languageRepository = languageRepository;
        _logger = logger;
    }

    public async Task<ServiceResult<List<LanguageDto>>> GetLanguagesAsync()
    {
        try
        {
            var languages = await _languageRepository.GetAll().ToListAsync();

            if (!languages.Any())
            {
                _logger.LogWarning("No languages found in the database.");
                return ServiceResult<List<LanguageDto>>.Fail("No languages found.", HttpStatusCode.NotFound);
            }

            var languageDtos = languages.Adapt<List<LanguageDto>>();
            _logger.LogInformation("Successfully retrieved {Count} languages.", languageDtos.Count);

            return ServiceResult<List<LanguageDto>>.Success(languageDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting languages.");
            return ServiceResult<List<LanguageDto>>.Fail("An error occurred while retrieving languages.", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ServiceResult<CompareLanguageIdResponse>> CompareLanguageIdAsync(CompareLanguageIdRequest request)
    {
        try
        {
            // Note: This implementation assumes we have access to user data.
            // Since User entity doesn't exist yet in this codebase, this is a placeholder.
            // This will need to be updated once Identity/User management is properly implemented.
            
            _logger.LogWarning("CompareLanguageId called but User entity is not implemented yet. Returning false.");
            
            // TODO: Implement actual comparison logic once User entity is available
            // var user = await _userRepository.GetByIdAsync(request.UserId);
            // if (user == null)
            // {
            //     return ServiceResult<CompareLanguageIdResponse>.Fail("User not found.", HttpStatusCode.NotFound);
            // }
            // var isMatch = user.NativeLanguageId == request.LanguageId;
            
            var response = new CompareLanguageIdResponse { IsMatch = false };
            return ServiceResult<CompareLanguageIdResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while comparing language ID.");
            return ServiceResult<CompareLanguageIdResponse>.Fail("An error occurred while comparing language.", HttpStatusCode.InternalServerError);
        }
    }
}
