using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR WRITING BOOK ENTITY.
/// </summary>
public interface IWritingBookRepository
{
    /// <summary>
    /// GETS A WRITING BOOK BY ID WITH ALL DETAILS INCLUDED.
    /// </summary>
    Task<WritingBook?> GetWritingBookItemByIdAsync(int id);

    /// <summary>
    /// GETS PAGED WRITING BOOKS FOR A USER WITH ALL DETAILS.
    /// </summary>
    Task<(List<WritingBook> Items, int TotalCount)> GetAllWBooksWithPagingAsync(string userId, int page, int pageSize);

    /// <summary>
    /// GETS WRITING BOOKS THAT CAN BE CREATED FOR A USER BASED ON LANGUAGE AND PRACTICE.
    /// </summary>
    Task<List<WritingBook>> GetWBookCreateItemsAsync(string userId, int languageId, int practiceId);

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(WritingBook entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<WritingBook?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE WRITING BOOK IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    WritingBook Update(WritingBook entity);

    /// <summary>
    /// REMOVES THE WRITING BOOK FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(WritingBook entity);
}
