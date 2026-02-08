using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingBooks.Dtos;
using App.Application.Features.WritingSessionRows.Dtos;
using App.Domain.Entities.WritingEntities;
using App.Domain.Exceptions;
using MapsterMapper;

namespace App.Application.Features.WritingSessionRows.Queries.GetWRowsByIdWithPaging;

public class GetWRowsByIdWithPagingQueryHandler(

    IWritingSessionRowRepository writingSessionRowRepository,
    IWritingOldSessionRepository writingOldSessionRepository,
    IWritingBookRepository writingBookRepository,
    IMapper mapper

    ) : IQueryHandler<GetWRowsByIdWithPagingQuery, ServiceResult<WritingRowsResponse>>
{
    public async Task<ServiceResult<WritingRowsResponse>> Handle(

        GetWRowsByIdWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        // FIND OLD SESSION
        var oldSession = await writingOldSessionRepository.GetByIdAsync(request.OldSessionId)
            ?? throw new NotFoundException("WRITING OLD SESSION NOT FOUND");

        // GET ROWS
        var rows = await writingSessionRowRepository.GetWritingRowsByIdWithPagingAsync(request.OldSessionId, request.Request.Page, request.Request.PageSize);

        // GET WRITING BOOK ITEM
        var writingBook = await writingBookRepository.GetByIdAsync(oldSession.WritingBookId);

        var writingBookItem = mapper.Map<WritingBook, WritingBookDto>(writingBook!);

        var result = mapper.Map<List<WritingSessionRow>, List<WritingSessionRowDto>>(rows.Items);

        var response = new WritingRowsResponse(

            Item: writingBookItem,
            Contents: result,
            Total: rows.TotalCount
            );

        return ServiceResult<WritingRowsResponse>.Success(response);
    }
}
