using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.FlashcardEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR FLASHCARD ENTITY.
/// </summary>
public class FlashcardRepository(AppDbContext context) : IFlashcardRepository
{
    public async Task AddAsync(Flashcard entity) => await context.Flashcards.AddAsync(entity);

    public async Task<Flashcard?> GetByIdAsync(int id) =>
        await context.Flashcards
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);

    public void Update(Flashcard entity) => context.Flashcards.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.Flashcards.FindAsync(id);

        if (entity is not null)
        {
            context.Flashcards.Remove(entity);
        }
    }

    public async Task<Flashcard?> GetByPracticeIdUserIdLanguageIdAsync(int practiceId, string userId, int languageId) =>
        await context.Flashcards
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.PracticeId == practiceId && f.UserId == userId && f.LanguageId == languageId);
}
