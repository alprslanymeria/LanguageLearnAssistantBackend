using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR READING BOOK ENTITY.
/// </summary>
public class ReadingBookRepository(AppDbContext context) : IReadingBookRepository
{
    public async Task<ReadingBook?> GetReadingBookItemByIdAsync(int id)
    {
        return await context.ReadingBooks
            .AsNoTracking()
            .Include(rb => rb.Reading)
                .ThenInclude(r => r.LanguageId)
            .FirstOrDefaultAsync(rb => rb.Id == id);
    }

    public async Task<(List<ReadingBook> Items, int TotalCount)> GetAllRBooksWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = context.ReadingBooks
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
        return await context.ReadingBooks
            .AsNoTracking()
            .Where(rb => rb.Reading.UserId == userId &&
                         rb.Reading.Language.Id == languageId &&
                         rb.Reading.Practice.Id == practiceId)
            .ToListAsync();
    }

    public async Task CreateAsync(ReadingBook entity) => await context.ReadingBooks.AddAsync(entity);


    public Task<ReadingBook?> GetByIdAsync(int id)
    {
        return context.ReadingBooks
            .AsNoTracking()
            .FirstOrDefaultAsync(rb => rb.Id == id);
    }

    public ReadingBook Update(ReadingBook entity)
    {
        context.ReadingBooks.Update(entity);

        return entity;
    }

    public void Delete(ReadingBook entity)
    {
        context.ReadingBooks
            .Remove(entity);
    }
}
