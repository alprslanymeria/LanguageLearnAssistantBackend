using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR WRITING ENTITY.
/// </summary>
public class WritingRepository(AppDbContext context) : IWritingRepository
{
    public async Task AddAsync(Writing entity) => await context.Writings.AddAsync(entity);

    public async Task<Writing?> GetByIdAsync(int id) =>
        await context.Writings
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id);

    public void Update(Writing entity) => context.Writings.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.Writings.FindAsync(id);

        if (entity is not null)
        {
            context.Writings.Remove(entity);
        }
    }

    public async Task<Writing?> GetByPracticeIdUserIdLanguageIdAsync(int practiceId, string userId, int languageId) =>
        await context.Writings
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.PracticeId == practiceId && w.UserId == userId && w.LanguageId == languageId);
}
