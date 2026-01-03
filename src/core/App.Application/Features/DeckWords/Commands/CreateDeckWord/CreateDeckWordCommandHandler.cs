using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.Dtos;
using App.Domain.Entities.FlashcardEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Features.DeckWords.Commands.CreateDeckWord;

/// <summary>
/// HANDLER FOR CREATE DECK WORD COMMAND.
/// </summary>
public class CreateDeckWordCommandHandler(
    IDeckWordRepository deckWordRepository,
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CreateDeckWordCommandHandler> logger
    ) : ICommandHandler<CreateDeckWordCommand, ServiceResult<DeckWordDto>>
{
    public async Task<ServiceResult<DeckWordDto>> Handle(
        CreateDeckWordCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateDeckWordCommandHandler -> CREATING NEW DECK WORD FOR CATEGORY ID: {CategoryId}", request.FlashcardCategoryId);

        // VERIFY FLASHCARD CATEGORY EXISTS
        var flashcardCategory = await flashcardCategoryRepository.GetByIdAsync(request.FlashcardCategoryId);

        if (flashcardCategory is null)
        {
            logger.LogWarning("CreateDeckWordCommandHandler -> FLASHCARD CATEGORY NOT FOUND WITH ID: {CategoryId}", request.FlashcardCategoryId);
            return ServiceResult<DeckWordDto>.Fail("FLASHCARD CATEGORY NOT FOUND", HttpStatusCode.NotFound);
        }

        try
        {
            // CREATE DECK WORD
            var deckWord = new DeckWord
            {
                FlashcardCategoryId = request.FlashcardCategoryId,
                Question = request.Question,
                Answer = request.Answer,
                FlashcardCategory = flashcardCategory
            };

            await deckWordRepository.CreateAsync(deckWord);
            await unitOfWork.CommitAsync();

            logger.LogInformation("CreateDeckWordCommandHandler -> SUCCESSFULLY CREATED DECK WORD WITH ID: {Id}", deckWord.Id);

            var result = mapper.Map<DeckWord, DeckWordDto>(deckWord);
            return ServiceResult<DeckWordDto>.SuccessAsCreated(result, $"/api/DeckWord/{deckWord.Id}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateDeckWordCommandHandler -> ERROR CREATING DECK WORD");
            return ServiceResult<DeckWordDto>.Fail("ERROR CREATING DECK WORD", HttpStatusCode.InternalServerError);
        }
    }
}
