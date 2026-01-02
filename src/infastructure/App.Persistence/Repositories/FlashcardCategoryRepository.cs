using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR FLASHCARD CATEGORY ENTITY.
/// </summary>
public class FlashcardCategoryRepository(AppDbContext context) : GenericRepository<FlashcardCategory, int>(context), IFlashcardCategoryRepository
{
    public async Task<FlashcardCategory?> GetFlashcardCategoryItemByIdAsync(int id)
    {
        return await Context.FlashcardCategories
            .AsNoTracking()
            .Include(fc => fc.Flashcard)
            .ThenInclude(f => f.LanguageId)
            .FirstOrDefaultAsync(fc => fc.Id == id);
    }

    public async Task<(List<FlashcardCategory> Items, int TotalCount)> GetAllFCategoriesWithPagingAsync(string userId, int page, int pageSize)
    {
        var query = Context.FlashcardCategories
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
        return await Context.FlashcardCategories
            .AsNoTracking()
            .Where(fc => fc.Flashcard.UserId == userId &&
                         fc.Flashcard.Language.Id == languageId &&
                         fc.Flashcard.Practice.Id == practiceId)
            .ToListAsync();
    }
}
