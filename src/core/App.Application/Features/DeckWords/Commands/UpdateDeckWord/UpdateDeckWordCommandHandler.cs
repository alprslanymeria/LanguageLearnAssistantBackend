using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.DeckWords.Commands.UpdateDeckWord;

/// <summary>
/// HANDLER FOR UPDATE DECK WORD COMMAND.
/// </summary>
public class UpdateDeckWordCommandHandler(
    IDeckWordRepository deckWordRepository,
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<UpdateDeckWordCommandHandler> logger
    ) : ICommandHandler<UpdateDeckWordCommand, ServiceResult<DeckWordDto>>
{
    public async Task<ServiceResult<DeckWordDto>> Handle(
        UpdateDeckWordCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateDeckWordCommandHandler -> UPDATING DECK WORD WITH ID: {Id}", request.Id);

        // VERIFY DECK WORD EXISTS
        var existingDeckWord = await deckWordRepository.GetByIdAsync(request.Id);

        if (existingDeckWord is null)
        {
            logger.LogWarning("UpdateDeckWordCommandHandler -> DECK WORD NOT FOUND WITH ID: {Id}", request.Id);
            return ServiceResult<DeckWordDto>.Fail("DECK WORD NOT FOUND", HttpStatusCode.NotFound);
        }

        // VERIFY FLASHCARD CATEGORY EXISTS
        var flashcardCategory = await flashcardCategoryRepository.GetByIdAsync(request.FlashcardCategoryId);

        if (flashcardCategory is null)
        {
            logger.LogWarning("UpdateDeckWordCommandHandler -> FLASHCARD CATEGORY NOT FOUND WITH ID: {CategoryId}", request.FlashcardCategoryId);
            return ServiceResult<DeckWordDto>.Fail("FLASHCARD CATEGORY NOT FOUND", HttpStatusCode.NotFound);
        }

        try
        {
            // UPDATE FIELDS
            existingDeckWord.FlashcardCategoryId = request.FlashcardCategoryId;
            existingDeckWord.Question = request.Question;
            existingDeckWord.Answer = request.Answer;

            deckWordRepository.Update(existingDeckWord);
            await unitOfWork.CommitAsync();

            logger.LogInformation("UpdateDeckWordCommandHandler -> SUCCESSFULLY UPDATED DECK WORD WITH ID: {Id}", existingDeckWord.Id);

            var result = mapper.Map<DeckWord, DeckWordDto>(existingDeckWord);
            return ServiceResult<DeckWordDto>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UpdateDeckWordCommandHandler -> ERROR UPDATING DECK WORD");
            return ServiceResult<DeckWordDto>.Fail("ERROR UPDATING DECK WORD", HttpStatusCode.InternalServerError);
        }
    }
}
