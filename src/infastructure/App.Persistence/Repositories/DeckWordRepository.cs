using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR DECK WORD ENTITY.
/// </summary>
public class DeckWordRepository(AppDbContext context) : GenericRepository<DeckWord, int>(context), IDeckWordRepository
{
    public async Task<DeckWord?> GetDeckWordItemByIdAsync(int id)
    {
        return await Context.DeckWords
            .AsNoTracking()
            .Include(dw => dw.FlashcardCategory)
                .ThenInclude(fc => fc.Flashcard)
                .ThenInclude(f => f.LanguageId)
            .FirstOrDefaultAsync(dw => dw.Id == id);
    }

    public async Task<(List<DeckWord> Items, int TotalCount)> GetAllDWordsWithPagingAsync(int categoryId, int page, int pageSize)
    {
        var query = Context.DeckWords
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

}
