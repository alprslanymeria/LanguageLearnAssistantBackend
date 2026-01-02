using App.Application.Common;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardCategories.Dtos;
using App.Application.Features.ReadingBooks.Dtos;
using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.ReadingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.FlashcardCategories;

/// <summary>
/// SERVICE IMPLEMENTATION FOR FLASHCARD CATEGORY OPERATIONS.
/// </summary>
public class FlashcardCategoryService(

    IFlashcardCategoryRepository flashcardCategoryRepository,
    IFlashcardRepository flashcardRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<FlashcardCategoryService> logger
    
    ) : IFlashcardCategoryService
{

    #region UTILS

    /// <summary>
    /// VERIFY OR CREATE FLASHCARD IF IT DOESN'T EXIST
    /// </summary>
    private async Task<ServiceResult<Flashcard>> VerifyOrCreateFlashcardAsync(int flashcardId, string userId, int languageId)
    {
        var flashcard = await flashcardRepository.GetByIdAsync(flashcardId);

        if (flashcard is not null)
        {
            return ServiceResult<Flashcard>.Success(flashcard);
        }

        logger.LogWarning("FlashcardCategoryService:VerifyOrCreateFlashcardAsync -> FLASHCARD NOT FOUND WITH ID: {FlashcardId}", flashcardId);

        var language = await languageRepository.GetByIdAsync(languageId);

        if (language is null)
        {
            logger.LogWarning("FlashcardCategoryService:VerifyOrCreateFlashcardAsync -> LANGUAGE NOT FOUND FOR ID: {LanguageId}", languageId);
            return ServiceResult<Flashcard>.Fail("LANGUAGE NOT FOUND", HttpStatusCode.NotFound);
        }

        var practice = await practiceRepository.ExistsByLanguageIdAsync(languageId);

        if (practice is null)
        {
            logger.LogWarning("FlashcardCategoryService:VerifyOrCreateFlashcardAsync -> PRACTICE NOT FOUND FOR LANGUAGE ID: {LanguageId}", languageId);
            return ServiceResult<Flashcard>.Fail("PRACTICE NOT FOUND FOR LANGUAGE", HttpStatusCode.NotFound);
        }

        flashcard = new Flashcard
        {
            UserId = userId,
            LanguageId = languageId,
            PracticeId = practice.Id,
            Language = language,
            Practice = practice
        };

        await flashcardRepository.CreateAsync(flashcard);

        logger.LogInformation("FlashcardCategoryService:VerifyOrCreateFlashcardAsync -> NEW FLASHCARD CREATED WITH ID: {FlashcardId}", flashcard.Id);

        return ServiceResult<Flashcard>.Success(flashcard);
    }


    #endregion


    public async Task<ServiceResult<FlashcardCategoryWithLanguageId>> GetFlashcardCategoryItemByIdAsync(int id)
    {
        logger.LogInformation("FlashcardCategoryService:GetFlashcardCategoryItemByIdAsync --> FETCHING FLASHCARD CATEGORY WITH ID: {Id}", id);

        var flashcardCategory = await flashcardCategoryRepository.GetFlashcardCategoryItemByIdAsync(id);

        if (flashcardCategory is null)
        {
            logger.LogWarning("FlashcardCategoryService:GetFlashcardCategoryItemByIdAsync --> FLASHCARD CATEGORY NOT FOUND WITH ID: {Id}", id);
            return ServiceResult<FlashcardCategoryWithLanguageId>.Fail("FLASHCARD CATEGORY NOT FOUND", HttpStatusCode.NotFound);
        }

        logger.LogInformation("FlashcardCategoryService:GetFlashcardCategoryItemByIdAsync --> SUCCESSFULLY FETCHED FLASHCARD CATEGORY: {CategoryName}", flashcardCategory.Name);

        var result = mapper.Map<FlashcardCategory, FlashcardCategoryWithLanguageId>(flashcardCategory);
        return ServiceResult<FlashcardCategoryWithLanguageId>.Success(result);
    }

    public async Task<ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>> GetAllFCategoriesWithPagingAsync(string userId, PagedRequest request)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            logger.LogWarning("FlashcardCategoryService:GetAllFCategoriesWithPagingAsync --> USER ID IS REQUIRED FOR FETCHING FLASHCARD CATEGORIES");
            return ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>.Fail("USER ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("FlashcardCategoryService:GetAllFCategoriesWithPagingAsync --> FETCHING FLASHCARD CATEGORIES FOR USER: {UserId}, PAGE: {Page}, PAGE SIZE: {PageSize}", userId, request.Page, request.PageSize);

        var (items, totalCount) = await flashcardCategoryRepository.GetAllFCategoriesWithPagingAsync(userId, request.Page, request.PageSize);

        logger.LogInformation("FlashcardCategoryService:GetAllFCategoriesWithPagingAsync --> FETCHED {Count} FLASHCARD CATEGORIES OUT OF {Total} FOR USER: {UserId}", items.Count, totalCount, userId);

        var mappedDtos = mapper.Map<List<FlashcardCategory>, List<FlashcardCategoryDto>>(items);
        var mappedResult = new FlashcardCategoryWithTotalCount
        {
            FlashcardCategoryDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<FlashcardCategoryWithTotalCount>.Create([mappedResult], request, totalCount);

        return ServiceResult<PagedResult<FlashcardCategoryWithTotalCount>>.Success(result);
    }

    public async Task<ServiceResult<List<FlashcardCategoryDto>>> GetFCategoryCreateItemsAsync(string userId, string language, string practice)
    {
        // GUARD CLAUSE
        if (string.IsNullOrWhiteSpace(userId))
        {
            logger.LogWarning("FlashcardCategoryService:GetFCategoryCreateItemsAsync --> USER ID IS REQUIRED FOR FETCHING CREATE ITEMS");
            return ServiceResult<List<FlashcardCategoryDto>>.Fail("USER ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("FlashcardCategoryService:GetFCategoryCreateItemsAsync --> FETCHING FLASHCARD CATEGORY CREATE ITEMS FOR USER: {UserId}", userId);

        // CHECK IF LANGUAGES EXIST
        var languageExists = await languageRepository.ExistsByNameAsync(language);

        if (languageExists is null)
        {
            logger.LogWarning("FlashcardCategoryService:GetFCategoryCreateItemsAsync --> LANGUAGE NOT FOUND: {Language}", language);
            return ServiceResult<List<FlashcardCategoryDto>>.Fail($"LANGUAGE '{language}' NOT FOUND",
                HttpStatusCode.NotFound);
        }

        // CHECK IF PRACTICE EXIST
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(practice, languageExists.Id);

        if (practiceExists is null)
        {
            logger.LogWarning("FlashcardCategoryService:GetFCategoryCreateItemsAsync --> PRACTICE NOT FOUND: {Practice} FOR LANGUAGE: {Language}", practice, language);
            return ServiceResult<List<FlashcardCategoryDto>>.Fail($"PRACTICE '{practice}' NOT FOUND FOR LANGUAGE '{language}'.",
                HttpStatusCode.NotFound);
        }

        var flashcardCategories = await flashcardCategoryRepository.GetFCategoryCreateItemsAsync(userId, languageExists.Id, practiceExists.Id);

        var result = mapper.Map<List<FlashcardCategory>, List<FlashcardCategoryDto>>(flashcardCategories);

        logger.LogInformation("FlashcardCategoryService:GetFCategoryCreateItemsAsync -> SUCCESSFULLY FETCHED {Count} CREATE ITEMS", result.Count);

        return ServiceResult<List<FlashcardCategoryDto>>.Success(result);
    }

    public async Task<ServiceResult> DeleteFCategoryItemByIdAsync(int id)
    {
        // GUARD CLAUSE
        if (id <= 0)
        {
            logger.LogWarning("FlashcardCategoryService:DeleteFCategoryItemByIdAsync -> INVALID FLASHCARD CATEGORY ID FOR DELETION: {Id}", id);
            return ServiceResult.Fail("INVALID FLASHCARD CATEGORY ID", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("FlashcardCategoryService:DeleteFCategoryItemByIdAsync -> ATTEMPTING TO DELETE FLASHCARD CATEGORY WITH ID: {Id}", id);

        var flashcardCategory = await flashcardCategoryRepository.GetByIdAsync(id);

        // FAST FAIL
        if (flashcardCategory is null)
        {
            logger.LogWarning("FlashcardCategoryService:DeleteFCategoryItemByIdAsync -> FLASHCARD CATEGORY NOT FOUND FOR DELETION WITH ID: {Id}", id);
            return ServiceResult.Fail("FLASHCARD CATEGORY NOT FOUND", HttpStatusCode.NotFound);
        }

        flashcardCategoryRepository.Delete(flashcardCategory);
        await unitOfWork.CommitAsync();

        logger.LogInformation("FlashcardCategoryService:DeleteFCategoryItemByIdAsync -> SUCCESSFULLY DELETED FLASHCARD CATEGORY FROM DATABASE WITH ID: {Id}", id);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult<FlashcardCategoryDto>> FlashcardCategoryAddAsync(CreateFlashcardCategoryRequest request)
    {
        logger.LogInformation("FlashcardCategoryService:FlashcardCategoryAddAsync -> CREATING NEW FLASHCARD CATEGORY FOR FLASHCARD ID: {FlashcardId}", request.FlashcardId);

        // VERIFY OR CREATE FLASHCARD
        var flashcardResult = await VerifyOrCreateFlashcardAsync(request.FlashcardId, request.UserId, request.LanguageId);

        // FAST FAIL
        if (!flashcardResult.IsSuccess)
        {
            return ServiceResult<FlashcardCategoryDto>.Fail(flashcardResult.ErrorMessage!, flashcardResult.Status);
        }

        var flashcard = flashcardResult.Data!;

        try
        {
            // CREATE FLASHCARD CATEGORY
            var flashcardCategory = new FlashcardCategory
            {
                FlashcardId = flashcard.Id,
                Name = request.Name,
                Flashcard = flashcard
            };

            await flashcardCategoryRepository.CreateAsync(flashcardCategory);
            await unitOfWork.CommitAsync();

            logger.LogInformation("FlashcardCategoryService:FlashcardCategoryAddAsync -> SUCCESSFULLY CREATED READING BOOK WITH ID: {Id}, NAME: {Name}", flashcardCategory.Id, flashcardCategory.Name);

            var result = mapper.Map<FlashcardCategory, FlashcardCategoryDto>(flashcardCategory);
            return ServiceResult<FlashcardCategoryDto>.SuccessAsCreated(result, $"/api/FlashcardCategory/{flashcardCategory.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "FlashcardCategoryService:FlashcardCategoryAddAsync -> ERROR CREATING FLASHCARD CATEGORY");
            return ServiceResult<FlashcardCategoryDto>.Fail("ERROR CREATING FLASHCARD CATEGORY", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ServiceResult<FlashcardCategoryDto>> FlashcardCategoryUpdateAsync(UpdateFlashcardCategoryRequest request)
    {
        logger.LogInformation("FlashcardCategoryService:FlashcardCategoryUpdateAsync -> UPDATING FLASHCARD CATEGORY WITH ID: {Id}", request.Id);

        // VERIFY FLASHCARD CATEGORY EXISTS
        var existingCategory = await flashcardCategoryRepository.GetByIdAsync(request.Id);

        // FAST FAIL
        if (existingCategory is null)
        {
            logger.LogWarning("FlashcardCategoryService:FlashcardCategoryUpdateAsync -> FLASHCARD CATEGORY NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<FlashcardCategoryDto>.Fail("FLASHCARD CATEGORY NOT FOUND", HttpStatusCode.NotFound);
        }

        // VERIFY OR CREATE FLASHCARD
        var flashcardResult = await VerifyOrCreateFlashcardAsync(request.FlashcardId, request.UserId, request.LanguageId);

        // FAST FAIL
        if (!flashcardResult.IsSuccess)
        {
            return ServiceResult<FlashcardCategoryDto>.Fail(flashcardResult.ErrorMessage!, flashcardResult.Status);
        }

        var flashcard = flashcardResult.Data!;

        try
        {
            // UPDATE OTHER FIELDS
            existingCategory.FlashcardId = flashcard.Id;
            existingCategory.Name = request.Name;

            flashcardCategoryRepository.Update(existingCategory);
            await unitOfWork.CommitAsync();

            logger.LogInformation("FlashcardCategoryService:FlashcardCategoryUpdateAsync -> SUCCESSFULLY UPDATED FLASHCARD CATEGORY WITH ID: {Id}", existingCategory.Id);

            var result = mapper.Map<FlashcardCategory, FlashcardCategoryDto>(existingCategory);
            return ServiceResult<FlashcardCategoryDto>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "FlashcardCategoryService:FlashcardCategoryUpdateAsync -> ERROR UPDATING FLASHCARD CATEGORY");
            return ServiceResult<FlashcardCategoryDto>.Fail("ERROR UPDATING FLASHCARD CATEGORY", HttpStatusCode.InternalServerError);
        }
    }
}
