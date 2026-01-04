using System.Net;
using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingBooks.Dtos;
using App.Domain.Entities.WritingEntities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Application.Features.WritingBooks.Queries.GetWBookCreateItems;

/// <summary>
/// HANDLER FOR GET WRITING BOOK CREATE ITEMS QUERY.
/// </summary>
public class GetWBookCreateItemsQueryHandler(

    IWritingBookRepository writingBookRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository,
    IMapper mapper,
    ILogger<GetWBookCreateItemsQueryHandler> logger

    ) : IQueryHandler<GetWBookCreateItemsQuery, ServiceResult<List<WritingBookDto>>>
{
    public async Task<ServiceResult<List<WritingBookDto>>> Handle(

        GetWBookCreateItemsQuery request, 
        CancellationToken cancellationToken)
    {

        logger.LogInformation("GetWBookCreateItemsQueryHandler --> FETCHING WRITING BOOK CREATE ITEMS FOR USER: {UserId}", request.UserId);

        // CHECK IF LANGUAGES EXIST
        var languageExists = await languageRepository.ExistsByNameAsync(request.Language);

        if (languageExists is null)
        {
            logger.LogWarning("GetWBookCreateItemsQueryHandler --> LANGUAGE NOT FOUND: {Language}", request.Language);
            return ServiceResult<List<WritingBookDto>>.Fail($"LANGUAGE '{request.Language}' NOT FOUND",
                HttpStatusCode.NotFound);
        }

        // CHECK IF PRACTICE EXIST
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Practice, languageExists.Id);

        if (practiceExists is null)
        {
            logger.LogWarning("GetWBookCreateItemsQueryHandler --> PRACTICE NOT FOUND: {Practice} FOR LANGUAGE: {Language}", request.Practice, request.Language);
            return ServiceResult<List<WritingBookDto>>.Fail($"PRACTICE '{request.Practice}' NOT FOUND FOR LANGUAGE '{request.Language}'.",
                HttpStatusCode.NotFound);
        }

        var writingBooks = await writingBookRepository.GetWBookCreateItemsAsync(request.UserId, languageExists.Id, practiceExists.Id);

        var result = mapper.Map<List<WritingBook>, List<WritingBookDto>>(writingBooks);

        logger.LogInformation("GetWBookCreateItemsQueryHandler -> SUCCESSFULLY FETCHED {Count} CREATE ITEMS", result.Count);

        return ServiceResult<List<WritingBookDto>>.Success(result);
    }
}
