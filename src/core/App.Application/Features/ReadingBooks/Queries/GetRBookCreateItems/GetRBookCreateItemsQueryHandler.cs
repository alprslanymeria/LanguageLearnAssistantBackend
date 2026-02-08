using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingBooks.Dtos;
using App.Domain.Entities.ReadingEntities;
using App.Domain.Exceptions;
using MapsterMapper;

namespace App.Application.Features.ReadingBooks.Queries.GetRBookCreateItems;

/// <summary>
/// HANDLER FOR GET READING BOOK CREATE ITEMS QUERY.
/// </summary>
public class GetRBookCreateItemsQueryHandler(

    IReadingBookRepository readingBookRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IMapper mapper

    ) : IQueryHandler<GetRBookCreateItemsQuery, ServiceResult<List<ReadingBookDto>>>
{
    public async Task<ServiceResult<List<ReadingBookDto>>> Handle(

        GetRBookCreateItemsQuery request,
        CancellationToken cancellationToken)
    {
        // CHECK IF LANGUAGES EXIST
        var languageExists = await languageRepository.ExistsByNameAsync(request.Language)
            ?? throw new NotFoundException($"LANGUAGE '{request.Language}' NOT FOUND");

        // CHECK IF PRACTICE EXIST
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Practice, languageExists.Id)
            ?? throw new NotFoundException($"PRACTICE '{request.Practice}' NOT FOUND FOR LANGUAGE '{request.Language}'.");

        var readingBooks = await readingBookRepository.GetRBookCreateItemsAsync(request.UserId, languageExists.Id, practiceExists.Id);

        var result = mapper.Map<List<ReadingBook>, List<ReadingBookDto>>(readingBooks);

        return ServiceResult<List<ReadingBookDto>>.Success(result);
    }
}
