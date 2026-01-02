using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR WRITING BOOK ENTITY.
/// </summary>
public interface IWritingBookRepository : IGenericRepository<WritingBook, int>
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
}
