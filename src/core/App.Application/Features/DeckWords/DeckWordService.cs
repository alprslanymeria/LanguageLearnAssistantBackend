using App.Application.Common;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.DeckWords;

/// <summary>
/// SERVICE IMPLEMENTATION FOR DECK WORD OPERATIONS.
/// </summary>
public class DeckWordService(

    IDeckWordRepository deckWordRepository,
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<DeckWordService> logger
    
    ) : IDeckWordService
{

    public async Task<ServiceResult<DeckWordWithLanguageId>> GetDeckWordItemByIdAsync(int id)
    {
        logger.LogInformation("DeckWordService:GetDeckWordItemByIdAsync --> FETCHING DECK WORD WITH ID: {Id}", id);

        var deckWord = await deckWordRepository.GetDeckWordItemByIdAsync(id);

        if (deckWord is null)
        {
            logger.LogWarning("DeckWordService:GetDeckWordItemByIdAsync --> DECK WORD NOT FOUND WITH ID: {Id}", id);
            return ServiceResult<DeckWordWithLanguageId>.Fail("DECK WORD NOT FOUND", HttpStatusCode.NotFound);
        }

        logger.LogInformation("DeckWordService:GetDeckWordItemByIdAsync --> SUCCESSFULLY FETCHED DECK WORD: {WordName}", deckWord.Question);

        var result = mapper.Map<DeckWord, DeckWordWithLanguageId>(deckWord);
        return ServiceResult<DeckWordWithLanguageId>.Success(result);
    }

    public async Task<ServiceResult<PagedResult<DeckWordWithTotalCount>>> GetAllDWordsWithPagingAsync(int categoryId, PagedRequest request)
    {
        if (categoryId <= 0)
        {
            logger.LogWarning("DeckWordService:GetAllDWordsWithPagingAsync --> CATEGORY ID IS REQUIRED FOR FETCHING DECK WORDS");
            return ServiceResult<PagedResult<DeckWordWithTotalCount>>.Fail("CATEGORY ID IS REQUIRED", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("DeckWordService:GetAllDWordsWithPagingAsync --> FETCHING DECK WORDS FOR CATEGORY: {CategoryId}, PAGE: {Page}, PAGE SIZE: {PageSize}", categoryId, request.Page, request.PageSize);

        var (items, totalCount) = await deckWordRepository.GetAllDWordsWithPagingAsync(categoryId, request.Page, request.PageSize);

        logger.LogInformation("DeckWordService:GetAllDWordsWithPagingAsync --> FETCHED {Count} DECK WORDS OUT OF {Total} FOR CATEGORY: {CategoryId}", items.Count, totalCount, categoryId);

        var mappedDtos = mapper.Map<List<DeckWord>, List<DeckWordDto>>(items);
        var mappedResult = new DeckWordWithTotalCount
        {
            DeckWordDtos = mappedDtos,
            TotalCount = totalCount
        };

        var result = PagedResult<DeckWordWithTotalCount>.Create([mappedResult], request, totalCount);

        return ServiceResult<PagedResult<DeckWordWithTotalCount>>.Success(result);
    }

    public async Task<ServiceResult> DeleteDWordItemByIdAsync(int id)
    {
        // GUARD CLAUSE
        if (id <= 0)
        {
            logger.LogWarning("DeckWordService:DeleteDWordItemByIdAsync -> INVALID DECK WORD ID FOR DELETION: {Id}", id);
            return ServiceResult.Fail("INVALID DECK WORD ID", HttpStatusCode.BadRequest);
        }

        logger.LogInformation("DeckWordService:DeleteDWordItemByIdAsync -> ATTEMPTING TO DELETE DECK WORD WITH ID: {Id}", id);

        var deckWord = await deckWordRepository.GetByIdAsync(id);

        // FAST FAIL
        if (deckWord is null)
        {
            logger.LogWarning("DeckWordService:DeleteDWordItemByIdAsync -> DECK WORD NOT FOUND FOR DELETION WITH ID: {Id}", id);
            return ServiceResult.Fail("DECK WORD NOT FOUND", HttpStatusCode.NotFound);
        }

        deckWordRepository.Delete(deckWord);
        await unitOfWork.CommitAsync();

        logger.LogInformation("DeckWordService:DeleteDWordItemByIdAsync -> SUCCESSFULLY DELETED DECK WORD FROM DATABASE WITH ID: {Id}", id);

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult<DeckWordDto>> DeckWordAddAsync(CreateDeckWordRequest request)
    {
        logger.LogInformation("DeckWordService:DeckWordAddAsync -> CREATING NEW DECK WORD FOR CATEGORY ID: {CategoryId}", request.FlashcardCategoryId);

        // IN HERE WE DON'T NEED TO CHECK PREVIOUS ENTRIES BECAUSE WE ARE MAKING A DEPENDENT SELECTION.
        // A MEAN USER CANNOT CREATE A WORD IF THERE IS NO CATEGORY.

        var category = await flashcardCategoryRepository.GetByIdAsync(request.FlashcardCategoryId);

        try
        {

            // CREATE DECK WORD
            var deckWord = new DeckWord
            {
                FlashcardCategoryId = category.Id,
                Question = request.Question,
                Answer = request.Answer,
                FlashcardCategory = category!
            };

            await deckWordRepository.CreateAsync(deckWord);
            await unitOfWork.CommitAsync();

            logger.LogInformation("DeckWordService:DeckWordAddAsync -> SUCCESSFULLY CREATED DECK WORD WITH ID: {Id}, QUESTION: {Question}", deckWord.Id, deckWord.Question);

            var result = mapper.Map<DeckWord, DeckWordDto>(deckWord);
            return ServiceResult<DeckWordDto>.SuccessAsCreated(result, $"/api/DeckWord/{deckWord.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "DeckWordService:DeckWordAddAsync -> ERROR CREATING DECK WORD");
            return ServiceResult<DeckWordDto>.Fail("ERROR CREATING DECK WORD", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ServiceResult<DeckWordDto>> DeckWordUpdateAsync(UpdateDeckWordRequest request)
    {
        logger.LogInformation("DeckWordService:DeckWordUpdateAsync -> UPDATING DECK WORD WITH ID: {Id}", request.Id);

        // VERIFY DECK WORD EXISTS
        var existingDeckWord = await deckWordRepository.GetByIdAsync(request.Id);

        // FAST FAIL
        if (existingDeckWord is null)
        {
            logger.LogWarning("DeckWordService:DeckWordUpdateAsync -> DECK WORD NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<DeckWordDto>.Fail("DECK WORD NOT FOUND", HttpStatusCode.NotFound);
        }
        

        try
        {

            // UPDATE OTHER FIELDS
            existingDeckWord.FlashcardCategoryId = request.FlashcardCategoryId;
            existingDeckWord.Question = request.Question;
            existingDeckWord.Answer = request.Answer;

            deckWordRepository.Update(existingDeckWord);
            await unitOfWork.CommitAsync();

            logger.LogInformation("DeckWordService:DeckWordUpdateAsync -> SUCCESSFULLY UPDATED DECK WORD WITH ID: {Id}", existingDeckWord.Id);

            var result = mapper.Map<DeckWord, DeckWordDto>(existingDeckWord);
            return ServiceResult<DeckWordDto>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "DeckWordService:DeckWordUpdateAsync -> ERROR UPDATING DECK WORD");
            return ServiceResult<DeckWordDto>.Fail("ERROR UPDATING DECK WORD", HttpStatusCode.InternalServerError);
        }
    }
}
