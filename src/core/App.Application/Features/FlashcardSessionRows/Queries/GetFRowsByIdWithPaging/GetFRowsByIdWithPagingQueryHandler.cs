using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.FlashcardSessionRows.Dtos;
using App.Domain.Entities.FlashcardEntities;
using App.Domain.Exceptions;
using MapsterMapper;

namespace App.Application.Features.FlashcardSessionRows.Queries.GetFRowsByIdWithPaging;

public class GetFRowsByIdWithPagingQueryHandler(

    IFlashcardSessionRowRepository flashcardSessionRowRepository,
    IFlashcardOldSessionRepository flashcardOldSessionRepository,
    IFlashcardCategoryRepository flashcardCategoryRepository,
    IMapper mapper

    ) : IQueryHandler<GetFRowsByIdWithPagingQuery, ServiceResult<FlashcardRowsResponse>>
{
    public async Task<ServiceResult<FlashcardRowsResponse>> Handle(

        GetFRowsByIdWithPagingQuery request,
        CancellationToken cancellationToken)
    {

        // FIND OLD SESSION
        var oldSession = await flashcardOldSessionRepository.GetByIdAsync(request.OldSessionId)
            ?? throw new NotFoundException("FLASHCARD OLD SESSION NOT FOUND");

        // GET ROWS
        var rows = await flashcardSessionRowRepository.GetFlashcardRowsByIdWithPagingAsync(request.OldSessionId, request.Request.Page, request.Request.PageSize);

        // GET FLASHCARD CATEGORY ITEM
        var flashcardCategoryItem = await flashcardCategoryRepository.GetByIdWithDeckWordsAsync(oldSession.FlashcardCategoryId);

        var result = mapper.Map<List<FlashcardSessionRow>, List<FlashcardSessionRowDto>>(rows.Items);

        var response = new FlashcardRowsResponse(

            Item: flashcardCategoryItem!,
            Contents: result,
            Total: rows.TotalCount
            );

        return ServiceResult<FlashcardRowsResponse>.Success(response);
    }
}
