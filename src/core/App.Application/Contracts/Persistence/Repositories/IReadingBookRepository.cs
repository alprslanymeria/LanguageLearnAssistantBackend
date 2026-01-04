using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

/// <summary>
/// REPOSITORY INTERFACE FOR READING BOOK ENTITY.
/// </summary>
public interface IReadingBookRepository
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

    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task CreateAsync(ReadingBook entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ReadingBook?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE READING BOOK IN THE UNDERLYING DATA STORE AND RETURNS THE UPDATED ENTITY.
    /// </summary>
    ReadingBook Update(ReadingBook entity);

    /// <summary>
    /// REMOVES THE READING BOOK FROM THE UNDERLYING DATA STORE.
    /// </summary>
    void Delete(ReadingBook entity);
}
