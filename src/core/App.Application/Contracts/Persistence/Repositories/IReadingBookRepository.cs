using App.Application.Features.ReadingBooks.Dtos;
using App.Domain.Entities.ReadingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IReadingBookRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(ReadingBook entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<ReadingBook?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE READING BOOK IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(ReadingBook entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE READING BOOK FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// GETS A READING BOOK BY ID WITH ALL DETAILS INCLUDED.
    /// </summary>
    Task<ReadingBookWithLanguageId?> GetReadingBookItemByIdAsync(int id);

    /// <summary>
    /// GETS PAGED READING BOOKS FOR A USER WITH ALL DETAILS.
    /// </summary>
    Task<(List<ReadingBook> Items, int TotalCount)> GetAllRBooksWithPagingAsync(string userId, int page, int pageSize);

    /// <summary>
    /// GETS READING BOOKS THAT CAN BE CREATED FOR A USER BASED ON LANGUAGE AND PRACTICE.
    /// </summary>
    Task<List<ReadingBook>> GetRBookCreateItemsAsync(string userId, int languageId, int practiceId);
}
