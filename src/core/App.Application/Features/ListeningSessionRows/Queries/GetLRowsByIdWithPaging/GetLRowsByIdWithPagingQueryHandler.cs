using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ListeningSessionRows.Dtos;
using App.Domain.Entities.ListeningEntities;
using App.Domain.Exceptions;
using MapsterMapper;

namespace App.Application.Features.ListeningSessionRows.Queries.GetLRowsByIdWithPaging;

public class GetLRowsByIdWithPagingQueryHandler(

    IListeningSessionRowRepository listeningSessionRowRepository,
    IListeningOldSessionRepository listeningOldSessionRepository,
    IListeningCategoryRepository listeningCategoryRepository,
    IMapper mapper

    ) : IQueryHandler<GetLRowsByIdWithPagingQuery, ServiceResult<ListeningRowsResponse>>
{
    public async Task<ServiceResult<ListeningRowsResponse>> Handle(

        GetLRowsByIdWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        // FIND OLD SESSION
        var oldSession = await listeningOldSessionRepository.GetByIdAsync(request.OldSessionId)
            ?? throw new NotFoundException("LISTENING OLD SESSION NOT FOUND");

        // GET ROWS
        var rows = await listeningSessionRowRepository.GetListeningRowsByIdWithPagingAsync(request.OldSessionId, request.Request.Page, request.Request.PageSize);

        // GET LISTENING BOOK ITEM
        var listeningCategoryItem = await listeningCategoryRepository.GetByIdWithDeckVideosAsync(oldSession.ListeningCategoryId);

        var result = mapper.Map<List<ListeningSessionRow>, List<ListeningSessionRowDto>>(rows.Items);

        var response = new ListeningRowsResponse(

            Item: listeningCategoryItem!,
            Contents: result,
            Total: rows.TotalCount
            );

        return ServiceResult<ListeningRowsResponse>.Success(response);
    }
}
