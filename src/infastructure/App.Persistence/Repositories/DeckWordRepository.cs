using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR DECK WORD ENTITY.
/// </summary>
public class DeckWordRepository(AppDbContext context) : IDeckWordRepository
{
    public async Task<DeckWord?> GetDeckWordItemByIdAsync(int id)
    {
        return await context.DeckWords
            .AsNoTracking()
            .Include(dw => dw.FlashcardCategory)
                .ThenInclude(fc => fc.Flashcard)
                .ThenInclude(f => f.LanguageId)
            .FirstOrDefaultAsync(dw => dw.Id == id);
    }

    public async Task<(List<DeckWord> Items, int TotalCount)> GetAllDWordsWithPagingAsync(int categoryId, int page, int pageSize)
    {
        var query = context.DeckWords
            .AsNoTracking()
            .Where(dw => dw.FlashcardCategoryId == categoryId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(dw => dw.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task CreateAsync(DeckWord entity) => await context.DeckWords.AddAsync(entity);

    public Task<DeckWord?> GetByIdAsync(int id) =>
        context.DeckWords
            .AsNoTracking()
            .FirstOrDefaultAsync(dw => dw.Id == id);

    public DeckWord Update(DeckWord entity)
    {
        context.DeckWords.Update(entity);

        return entity;
    }

    public void Delete(DeckWord entity)
    {
        context.DeckWords
            .Remove(entity);
    }
}
