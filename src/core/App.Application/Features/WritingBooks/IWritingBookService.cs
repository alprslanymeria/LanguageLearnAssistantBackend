using App.Application.Common;
using App.Application.Features.WritingBooks.Dtos;

namespace App.Application.Features.WritingBooks;

/// <summary>
/// SERVICE INTERFACE FOR WRITING BOOK OPERATIONS.
/// </summary>
public interface IWritingBookService
{
    /// <summary>
    /// RETRIEVES A WRITING BOOK BY ID.
    /// </summary>
    Task<ServiceResult<WritingBookWithLanguageId>> GetWritingBookItemByIdAsync(int id);

    /// <summary>
    /// RETRIEVES ALL WRITING BOOKS WITH PAGING.
    /// </summary>
    Task<ServiceResult<PagedResult<WritingBookWithTotalCount>>> GetAllWBooksWithPagingAsync(string userId, PagedRequest request);

    /// <summary>
    /// RETRIEVES CREATE ITEMS FOR DROPDOWN SELECTIONS.
    /// </summary>
    Task<ServiceResult<List<WritingBookDto>>> GetWBookCreateItemsAsync(string userId, string language, string practice);

    /// <summary>
    /// DELETES A WRITING BOOK BY ID.
    /// </summary>
    Task<ServiceResult> DeleteWBookItemByIdAsync(int id);

    /// <summary>
    /// CREATES A NEW WRITING BOOK.
    /// </summary>
    Task<ServiceResult<WritingBookDto>> WritingBookAddAsync(CreateWritingBookRequest request);

    /// <summary>
    /// UPDATES AN EXISTING WRITING BOOK.
    /// </summary>
    Task<ServiceResult<WritingBookDto>> WritingBookUpdateAsync(UpdateWritingBookRequest request);
}
