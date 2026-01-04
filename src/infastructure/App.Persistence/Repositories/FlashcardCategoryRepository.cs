using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR FLASHCARD CATEGORY ENTITY.
/// </summary>
public class FlashcardCategoryRepository(AppDbContext context) : IFlashcardCategoryRepository
{
    public async Task<FlashcardCategory?> GetFlashcardCategoryItemByIdAsync(int id)
    {
        return await context.FlashcardCategories
            .AsNoTracking()
            .Include(fc => fc.Flashcard)
            .ThenInclude(f => f.LanguageId)
            .FirstOrDefaultAsync(fc => fc.Id == id);
    }

    public async Task<(List<FlashcardCategory> Items, int TotalCount)> GetAllFCategoriesWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = context.FlashcardCategories
            .AsNoTracking()
            .Where(fc => fc.Flashcard.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(fc => fc.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<FlashcardCategory>> GetFCategoryCreateItemsAsync(string userId, int languageId, int practiceId)
    {
        return await context.FlashcardCategories
            .AsNoTracking()
            .Where(fc => fc.Flashcard.UserId == userId &&
                         fc.Flashcard.Language.Id == languageId &&
                         fc.Flashcard.Practice.Id == practiceId)
            .ToListAsync();
    }

    public async Task CreateAsync(FlashcardCategory entity) => await context.FlashcardCategories.AddAsync(entity);

    public Task<FlashcardCategory?> GetByIdAsync(int id)
    {
        return context.FlashcardCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(fc => fc.Id == id);
    }

    public FlashcardCategory Update(FlashcardCategory entity)
    {
        context.FlashcardCategories.Update(entity);

        return entity;
    }

    public void Delete(FlashcardCategory entity)
    {
        context.FlashcardCategories
            .Remove(entity);
    }
}
