using App.Application.Features.ListeningCategories.Dtos;
using App.Domain.Entities.ListeningEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IListeningCategoryRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(ListeningCategory entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ListeningCategory?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE LISTENING CATEGORY IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(ListeningCategory entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE LISTENING CATEGORY FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// GETS LISTENING CATEGORIES WITH DECK VIDEOS FOR A USER BASED ON LANGUAGE AND PRACTICE.
    /// </summary>
    Task<List<ListeningCategoryWithDeckVideos>> GetLCategoryCreateItemsAsync(string userId, int languageId, int practiceId);

    /// <summary>
    /// GETS A LISTENING CATEGORY BY ID WITH DECK VIDEOS INCLUDED.
    /// </summary>
    Task<ListeningCategoryWithDeckVideos?> GetByIdWithDeckVideosAsync(int id);
}
