using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingBooks.Dtos;
using App.Application.Features.ReadingSessionRows.Dtos;
using App.Domain.Entities.ReadingEntities;
using App.Domain.Exceptions;
using MapsterMapper;

namespace App.Application.Features.ReadingSessionRows.Queries.GetRRowsByIdWithPaging;

public class GetRRowsByIdWithPagingQueryHandler(

    IReadingSessionRowRepository readingSessionRowRepository,
    IReadingOldSessionRepository readingOldSessionRepository,
    IReadingBookRepository readingBookRepository,
    IMapper mapper

    ) : IQueryHandler<GetRRowsByIdWithPagingQuery, ServiceResult<ReadingRowsResponse>>
{
    public async Task<ServiceResult<ReadingRowsResponse>> Handle(

        GetRRowsByIdWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        // FIND OLD SESSION
        var oldSession = await readingOldSessionRepository.GetByIdAsync(request.OldSessionId)
            ?? throw new NotFoundException("READING OLD SESSION NOT FOUND");

        // GET ROWS
        var rows = await readingSessionRowRepository.GetReadingRowsByIdWithPagingAsync(request.OldSessionId, request.Request.Page, request.Request.PageSize);

        // GET READING BOOK ITEM
        var readingBook = await readingBookRepository.GetByIdAsync(oldSession.ReadingBookId);

        var readingBookItem = mapper.Map<ReadingBook, ReadingBookDto>(readingBook!);

        var result = mapper.Map<List<ReadingSessionRow>, List<ReadingSessionRowDto>>(rows.Items);

        var response = new ReadingRowsResponse(

            Item: readingBookItem,
            Contents: result,
            Total: rows.TotalCount
            );

        return ServiceResult<ReadingRowsResponse>.Success(response);
    }
}
