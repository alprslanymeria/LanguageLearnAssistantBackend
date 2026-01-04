using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR WRITING BOOK ENTITY.
/// </summary>
public class WritingBookRepository(AppDbContext context) : IWritingBookRepository
{
    public async Task<WritingBook?> GetWritingBookItemByIdAsync(int id)
    {
        return await context.WritingBooks
            .AsNoTracking()
            .Include(wb => wb.Writing)
            .ThenInclude(w => w.Language)
            .FirstOrDefaultAsync(wb => wb.Id == id);
    }

    public async Task<(List<WritingBook> Items, int TotalCount)> GetAllWBooksWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = context.WritingBooks
            .AsNoTracking()
            .Where(wb => wb.Writing.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(wb => wb.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<WritingBook>> GetWBookCreateItemsAsync(string userId, int languageId, int practiceId)
    {
        return await context.WritingBooks
            .AsNoTracking()
            .Where(wb => wb.Writing.UserId == userId &&
                         wb.Writing.Language.Id == languageId &&
                         wb.Writing.Practice.Id == practiceId)
            .ToListAsync();
    }

    public async Task CreateAsync(WritingBook entity) => await context.WritingBooks.AddAsync(entity);


    public Task<WritingBook?> GetByIdAsync(int id)
    {
        return context.WritingBooks
            .AsNoTracking()
            .FirstOrDefaultAsync(wb => wb.Id == id);
    }

    public WritingBook Update(WritingBook entity)
    {
        context.WritingBooks.Update(entity);

        return entity;
    }

    public void Delete(WritingBook entity)
    {
        context.WritingBooks
            .Remove(entity);
    }
}
