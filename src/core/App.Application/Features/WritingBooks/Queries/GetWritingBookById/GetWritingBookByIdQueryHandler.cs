using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingBooks.Dtos;
using App.Domain.Exceptions;

namespace App.Application.Features.WritingBooks.Queries.GetWritingBookById;

/// <summary>
/// HANDLER FOR GET WRITING BOOK BY ID QUERY.
/// </summary>
public class GetWritingBookByIdQueryHandler(

    IWritingBookRepository writingBookRepository

    ) : IQueryHandler<GetWritingBookByIdQuery, ServiceResult<WritingBookWithLanguageId>>
{
    public async Task<ServiceResult<WritingBookWithLanguageId>> Handle(

        GetWritingBookByIdQuery request,
        CancellationToken cancellationToken)
    {

        // GET WRITING BOOK
        var writingBook = await writingBookRepository.GetWritingBookItemByIdAsync(request.Id)
            ?? throw new NotFoundException("WRITING BOOK NOT FOUND");

        return ServiceResult<WritingBookWithLanguageId>.Success(writingBook);
    }
}
