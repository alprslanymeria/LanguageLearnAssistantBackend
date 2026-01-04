using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.ListeningEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR LISTENING CATEGORY ENTITY.
/// </summary>
public interface IListeningCategoryRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(ListeningCategory entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ListeningCategory?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE LISTENING CATEGORY IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    ListeningCategory Update(ListeningCategory entity);

    /// <summary>
    /// REMOVES THE LISTENING CATEGORY FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(ListeningCategory entity);
}
