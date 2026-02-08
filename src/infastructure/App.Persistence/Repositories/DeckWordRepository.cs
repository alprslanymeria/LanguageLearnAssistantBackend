using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.Dtos;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR DECK WORD ENTITY.
/// </summary>
public class DeckWordRepository(AppDbContext context) : IDeckWordRepository
{
    public async Task AddAsync(DeckWord entity) => await context.DeckWords.AddAsync(entity);

    public async Task<DeckWord?> GetByIdAsync(int id) =>
        await context.DeckWords
            .AsNoTracking()
            .FirstOrDefaultAsync(dw => dw.Id == id);

    public void Update(DeckWord entity) => context.DeckWords.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.DeckWords.FindAsync(id);

        if (entity is not null)
        {
            context.DeckWords.Remove(entity);
        }
    }

    public async Task<DeckWordWithLanguageId?> GetDeckWordItemByIdAsync(int id)
    {
        return await context.DeckWords
            .Where(dw => dw.Id == id)
            .Select(dw => new DeckWordWithLanguageId(
                dw.Id,
                dw.FlashcardCategoryId,
                dw.Question,
                dw.Answer,
                dw.FlashcardCategory.Flashcard.Language.Id
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<(List<DeckWord> Items, int TotalCount)> GetAllDWordsWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = context.DeckWords
            .Where(dw => dw.FlashcardCategory.Flashcard.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(dw => dw.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
