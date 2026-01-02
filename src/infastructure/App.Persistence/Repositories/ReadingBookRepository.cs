using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR READING BOOK ENTITY.
/// </summary>
public class ReadingBookRepository(AppDbContext context) : GenericRepository<ReadingBook, int>(context), IReadingBookRepository
{
    public async Task<ReadingBook?> GetReadingBookItemByIdAsync(int id)
    {
        return await Context.ReadingBooks
            .AsNoTracking()
            .Include(rb => rb.Reading)
                .ThenInclude(r => r.LanguageId)
            .FirstOrDefaultAsync(rb => rb.Id == id);
    }

    public async Task<(List<ReadingBook> Items, int TotalCount)> GetAllRBooksWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = Context.ReadingBooks
            .AsNoTracking()
            .Where(rb => rb.Reading.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(rb => rb.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<ReadingBook>> GetRBookCreateItemsAsync(string userId, int languageId, int practiceId)
    {
        return await Context.ReadingBooks
            .AsNoTracking()
            .Where(rb => rb.Reading.UserId == userId &&
                         rb.Reading.Language.Id == languageId &&
                         rb.Reading.Practice.Id == practiceId)
            .ToListAsync();
    }
}
