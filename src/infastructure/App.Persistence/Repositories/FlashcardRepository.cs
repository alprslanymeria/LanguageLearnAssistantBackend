using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR FLASHCARD ENTITY.
/// </summary>
public class FlashcardRepository(AppDbContext context) : GenericRepository<Flashcard, int>(context), IFlashcardRepository
{

    public async Task<List<Flashcard>> GetByUserIdWithDetailsAsync(string userId)
    {
        return await Context.Flashcards
            .AsNoTracking()
            .Include(f => f.Language)
            .Include(f => f.Practice)
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }
}
