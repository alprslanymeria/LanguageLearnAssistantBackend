using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingBooks.Dtos;
using App.Domain.Entities.WritingEntities;
using App.Domain.Exceptions;
using MapsterMapper;

namespace App.Application.Features.WritingBooks.Queries.GetWBookCreateItems;

/// <summary>
/// HANDLER FOR GET WRITING BOOK CREATE ITEMS QUERY.
/// </summary>
public class GetWBookCreateItemsQueryHandler(

    IWritingBookRepository writingBookRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IMapper mapper

    ) : IQueryHandler<GetWBookCreateItemsQuery, ServiceResult<List<WritingBookDto>>>
{
    public async Task<ServiceResult<List<WritingBookDto>>> Handle(

        GetWBookCreateItemsQuery request,
        CancellationToken cancellationToken)
    {
        // CHECK IF LANGUAGES EXIST
        var languageExists = await languageRepository.ExistsByNameAsync(request.Language)
            ?? throw new NotFoundException($"LANGUAGE '{request.Language}' NOT FOUND");

        // CHECK IF PRACTICE EXIST
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Practice, languageExists.Id)
            ?? throw new NotFoundException($"PRACTICE '{request.Practice}' NOT FOUND FOR LANGUAGE '{request.Language}'.");

        var writingBooks = await writingBookRepository.GetWBookCreateItemsAsync(request.UserId, languageExists.Id, practiceExists.Id);

        var result = mapper.Map<List<WritingBook>, List<WritingBookDto>>(writingBooks);

        return ServiceResult<List<WritingBookDto>>.Success(result);
    }
}
