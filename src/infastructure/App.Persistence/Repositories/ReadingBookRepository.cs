using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.ReadingBooks.Dtos;
using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR READING BOOK ENTITY.
/// </summary>
public class ReadingBookRepository(AppDbContext context) : IReadingBookRepository
{
    public async Task AddAsync(ReadingBook entity) => await context.ReadingBooks.AddAsync(entity);

    public async Task<ReadingBook?> GetByIdAsync(int id) =>
        await context.ReadingBooks
            .AsNoTracking()
            .FirstOrDefaultAsync(rb => rb.Id == id);

    public void Update(ReadingBook entity) => context.ReadingBooks.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.ReadingBooks.FindAsync(id);

        if (entity is not null)
        {
            context.ReadingBooks.Remove(entity);
        }
    }

    public async Task<ReadingBookWithLanguageId?> GetReadingBookItemByIdAsync(int id)
    {
        return await context.ReadingBooks
            .Where(rb => rb.Id == id)
            .Select(rb => new ReadingBookWithLanguageId(
                rb.Id,
                rb.ReadingId,
                rb.Name,
                rb.ImageUrl,
                rb.LeftColor,
                rb.SourceUrl,
                rb.Reading.Language.Id
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<(List<ReadingBook> Items, int TotalCount)> GetAllRBooksWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = context.ReadingBooks
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
            .Where(rb =>
                rb.Reading.UserId == userId &&
                rb.Reading.LanguageId == languageId &&
                rb.Reading.PracticeId == practiceId)
            .AsNoTracking()
            .ToListAsync();
    }
}
