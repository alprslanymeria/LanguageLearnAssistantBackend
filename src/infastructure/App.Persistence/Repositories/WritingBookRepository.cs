using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.WritingBooks.Dtos;
using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR WRITING BOOK ENTITY.
/// </summary>
public class WritingBookRepository(AppDbContext context) : IWritingBookRepository
{
    public async Task AddAsync(WritingBook entity) => await context.WritingBooks.AddAsync(entity);

    public async Task<WritingBook?> GetByIdAsync(int id) =>
        await context.WritingBooks
            .AsNoTracking()
            .FirstOrDefaultAsync(wb => wb.Id == id);

    public void Update(WritingBook entity) => context.WritingBooks.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.WritingBooks.FindAsync(id);

        if (entity is not null)
        {
            context.WritingBooks.Remove(entity);
        }
    }

    public async Task<WritingBookWithLanguageId?> GetWritingBookItemByIdAsync(int id)
    {
        return await context.WritingBooks
            .Where(wb => wb.Id == id)
            .Select(wb => new WritingBookWithLanguageId(
                wb.Id,
                wb.WritingId,
                wb.Name,
                wb.ImageUrl,
                wb.LeftColor,
                wb.SourceUrl,
                wb.Writing.Language.Id
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<(List<WritingBook> Items, int TotalCount)> GetAllWBooksWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = context.WritingBooks
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
            .Where(wb =>
                wb.Writing.UserId == userId &&
                wb.Writing.LanguageId == languageId &&
                wb.Writing.PracticeId == practiceId)
            .AsNoTracking()
            .ToListAsync();
    }
}
