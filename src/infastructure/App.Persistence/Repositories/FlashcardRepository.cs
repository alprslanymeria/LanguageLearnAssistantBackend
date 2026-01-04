using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR FLASHCARD ENTITY.
/// </summary>
public class FlashcardRepository(AppDbContext context) : IFlashcardRepository
{

    public async Task<List<Flashcard>> GetByUserIdWithDetailsAsync(string userId)
    {
        return await context.Flashcards
            .AsNoTracking()
            .Include(f => f.Language)
            .Include(f => f.Practice)
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }

    public async Task CreateAsync(Flashcard entity) => await context.Flashcards.AddAsync(entity);

    public Task<Flashcard?> GetByIdAsync(int id) =>
        context.Flashcards
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);

    public Flashcard Update(Flashcard entity)
    {
        context.Flashcards.Update(entity);

        return entity;
    }

    public void Delete(Flashcard entity)
    {
        context.Flashcards
            .Remove(entity);
    }
}
