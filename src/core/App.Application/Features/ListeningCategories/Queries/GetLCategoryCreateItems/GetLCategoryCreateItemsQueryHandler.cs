using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ListeningCategories.Dtos;
using App.Domain.Exceptions;

namespace App.Application.Features.ListeningCategories.Queries.GetLCategoryCreateItems;

public class GetLCategoryCreateItemsQueryHandler(

    IListeningCategoryRepository listeningCategoryRepository,
    ILanguageRepository languageRepository,
    IPracticeRepository practiceRepository

    ) : IQueryHandler<GetLCategoryCreateItemsQuery, ServiceResult<List<ListeningCategoryWithDeckVideos>>>
{
    public async Task<ServiceResult<List<ListeningCategoryWithDeckVideos>>> Handle(

        GetLCategoryCreateItemsQuery request,
        CancellationToken cancellationToken)
    {
        // CHECK IF LANGUAGES EXIST
        var languageExists = await languageRepository.ExistsByNameAsync(request.Language)
            ?? throw new NotFoundException($"LANGUAGE '{request.Language}' NOT FOUND");

        // CHECK IF PRACTICE EXIST
        var practiceExists = await practiceRepository.ExistsByNameAndLanguageIdAsync(request.Practice, languageExists.Id)
            ?? throw new NotFoundException($"PRACTICE '{request.Practice}' NOT FOUND FOR LANGUAGE '{request.Language}'.");

        var ListeningCategories = await listeningCategoryRepository.GetLCategoryCreateItemsAsync(request.UserId, languageExists.Id, practiceExists.Id);

        return ServiceResult<List<ListeningCategoryWithDeckVideos>>.Success(ListeningCategories);

    }
}
