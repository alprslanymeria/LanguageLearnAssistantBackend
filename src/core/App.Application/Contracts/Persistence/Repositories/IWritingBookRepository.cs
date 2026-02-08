using App.Application.Features.WritingBooks.Dtos;
using App.Domain.Entities.WritingEntities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IWritingBookRepository
{
    /// <summary>
    /// ASYNCHRONOUSLY CREATES A NEW ENTITY IN THE UNDERLYING DATA STORE.
    /// </summary>
    Task AddAsync(WritingBook entity);

    /// <summary>
    /// ASYNCHRONOUSLY RETRIEVES AN ENTITY BY ITS UNIQUE IDENTIFIER.
    /// </summary>
    Task<WritingBook?> GetByIdAsync(int id);

    /// <summary>
    /// UPDATES THE WRITING BOOK IN THE UNDERLYING DATA STORE.
    /// </summary>
    void Update(WritingBook entity);

    /// <summary>
    /// ASYNCHRONOUSLY REMOVES THE WRITING BOOK FROM THE UNDERLYING DATA STORE.
    /// </summary>
    Task RemoveAsync(int id);

    /// <summary>
    /// GETS A WRITING BOOK BY ID WITH ALL DETAILS INCLUDED.
    /// </summary>
    Task<WritingBookWithLanguageId?> GetWritingBookItemByIdAsync(int id);

    /// <summary>
    /// GETS PAGED WRITING BOOKS FOR A USER WITH ALL DETAILS.
    /// </summary>
    Task<(List<WritingBook> Items, int TotalCount)> GetAllWBooksWithPagingAsync(string userId, int page, int pageSize);

    /// <summary>
    /// GETS WRITING BOOKS THAT CAN BE CREATED FOR A USER BASED ON LANGUAGE AND PRACTICE.
    /// </summary>
    Task<List<WritingBook>> GetWBookCreateItemsAsync(string userId, int languageId, int practiceId);
}
