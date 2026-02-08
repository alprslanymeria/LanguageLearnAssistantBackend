using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR READING ENTITY.
/// </summary>
public class ReadingRepository(AppDbContext context) : IReadingRepository
{
    public async Task AddAsync(Reading entity) => await context.Readings.AddAsync(entity);

    public async Task<Reading?> GetByIdAsync(int id) =>
        await context.Readings
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

    public void Update(Reading entity) => context.Readings.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.Readings.FindAsync(id);

        if (entity is not null)
        {
            context.Readings.Remove(entity);
        }
    }

    public async Task<Reading?> GetByPracticeIdUserIdLanguageIdAsync(int practiceId, string userId, int languageId) =>
        await context.Readings
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.PracticeId == practiceId && r.UserId == userId && r.LanguageId == languageId);
}
