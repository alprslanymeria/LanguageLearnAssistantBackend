using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.DeckWords.Dtos;
using App.Application.Features.FlashcardCategories.Dtos;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR FLASHCARD CATEGORY ENTITY.
/// </summary>
public class FlashcardCategoryRepository(AppDbContext context) : IFlashcardCategoryRepository
{
    public async Task AddAsync(FlashcardCategory entity) => await context.FlashcardCategories.AddAsync(entity);

    public async Task<FlashcardCategory?> GetByIdAsync(int id) =>
        await context.FlashcardCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(fc => fc.Id == id);

    public void Update(FlashcardCategory entity) => context.FlashcardCategories.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.FlashcardCategories.FindAsync(id);

        if (entity is not null)
        {
            context.FlashcardCategories.Remove(entity);
        }
    }

    public async Task<FlashcardCategoryWithLanguageId?> GetFlashcardCategoryItemByIdAsync(int id)
    {
        return await context.FlashcardCategories
            .Where(fc => fc.Id == id)
            .Select(fc => new FlashcardCategoryWithLanguageId(
                fc.Id,
                fc.FlashcardId,
                fc.Name,
                fc.Flashcard.Language.Id
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<(List<FlashcardCategoryWithLanguageId> Items, int TotalCount)> GetAllFCategoriesAsync(string userId)
    {
        var query = context.FlashcardCategories
            .Where(fc => fc.Flashcard.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(fc => fc.Id)
            .Select(fc => new FlashcardCategoryWithLanguageId(
                fc.Id,
                fc.FlashcardId,
                fc.Name,
                fc.Flashcard.Language.Id
            ))
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<FlashcardCategory> Items, int TotalCount)> GetAllFCategoriesWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = context.FlashcardCategories
            .Where(fc => fc.Flashcard.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(fc => fc.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<FlashcardCategoryWithDeckWords>> GetFCategoryCreateItemsAsync(string userId, int languageId, int practiceId)
    {
        return await context.FlashcardCategories
            .Where(fc =>
                fc.Flashcard.UserId == userId &&
                fc.Flashcard.LanguageId == languageId &&
                fc.Flashcard.PracticeId == practiceId &&
                fc.DeckWords.Any())
            .Select(fc => new FlashcardCategoryWithDeckWords(
                fc.Id,
                fc.FlashcardId,
                fc.Name,
                fc.DeckWords
                    .Select(dw => new DeckWordDto(
                        dw.Id,
                        dw.FlashcardCategoryId,
                        dw.Question,
                        dw.Answer
                    ))
                    .ToList()
            ))
            .ToListAsync();
    }

    public async Task<FlashcardCategoryWithDeckWords?> GetByIdWithDeckWordsAsync(int id)
    {
        return await context.FlashcardCategories
            .Where(fc => fc.Id == id)
            .Select(fc => new FlashcardCategoryWithDeckWords(
                fc.Id,
                fc.FlashcardId,
                fc.Name,
                fc.DeckWords
                    .Select(dw => new DeckWordDto(
                        dw.Id,
                        dw.FlashcardCategoryId,
                        dw.Question,
                        dw.Answer
                    ))
                    .ToList()
            ))
            .FirstOrDefaultAsync();
    }
}
