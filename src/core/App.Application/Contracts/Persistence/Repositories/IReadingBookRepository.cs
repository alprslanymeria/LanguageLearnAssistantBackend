using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR READING BOOK ENTITY.
/// </summary>
public interface IReadingBookRepository : IGenericRepository<ReadingBook, int>
{
    /// <summary>
    /// GETS A READING BOOK BY ID WITH ALL DETAILS INCLUDED.
    /// </summary>
    Task<ReadingBook?> GetReadingBookItemByIdAsync(int id);

    /// <summary>
    /// GETS PAGED READING BOOKS FOR A USER WITH ALL DETAILS.
    /// </summary>
    Task<(List<ReadingBook> Items, int TotalCount)> GetAllRBooksWithPagingAsync(string userId, int page, int pageSize);

    /// <summary>
    /// GETS READING BOOKS THAT CAN BE CREATED FOR A USER BASED ON LANGUAGE AND PRACTICE.
    /// </summary>
    Task<List<ReadingBook>> GetRBookCreateItemsAsync(string userId, int languageId, int practiceId);
}
