using App.Application.Common;
using App.Application.Features.ReadingBooks.Dtos;

namespace App.Application.Features.ReadingBooks;

/// <summary>
/// SERVICE INTERFACE FOR READING BOOK OPERATIONS.
/// </summary>
public interface IReadingBookService
{
    /// <summary>
    /// RETRIEVES A READING BOOK BY ID.
    /// </summary>
    Task<ServiceResult<ReadingBookWithLanguageId>> GetReadingBookItemByIdAsync(int id);

    /// <summary>
    /// RETRIEVES ALL READING BOOKS WITH PAGING.
    /// </summary>
    Task<ServiceResult<PagedResult<ReadingBookWithTotalCount>>> GetAllRBooksWithPagingAsync(string userId, PagedRequest request);

    /// <summary>
    /// RETRIEVES CREATE ITEMS FOR DROPDOWN SELECTIONS.
    /// </summary>
    Task<ServiceResult<List<ReadingBookDto>>> GetRBookCreateItemsAsync(string userId, string language, string practice);

    /// <summary>
    /// DELETES A READING BOOK BY ID.
    /// </summary>
    Task<ServiceResult> DeleteRBookItemByIdAsync(int id);

    /// <summary>
    /// CREATES A NEW READING BOOK.
    /// </summary>
    Task<ServiceResult<ReadingBookDto>> ReadingBookAddAsync(CreateReadingBookRequest request);

    /// <summary>
    /// UPDATES AN EXISTING READING BOOK.
    /// </summary>
    Task<ServiceResult<ReadingBookDto>> ReadingBookUpdateAsync(UpdateReadingBookRequest request);
}
