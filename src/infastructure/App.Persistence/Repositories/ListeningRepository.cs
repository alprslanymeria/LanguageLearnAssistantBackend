using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ListeningEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LISTENING ENTITY.
/// </summary>
public class ListeningRepository(AppDbContext context) : IListeningRepository
{
    public async Task AddAsync(Listening entity) => await context.Listenings.AddAsync(entity);

    public async Task<Listening?> GetByIdAsync(int id) =>
        await context.Listenings
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

    public void Update(Listening entity) => context.Listenings.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.Listenings.FindAsync(id);

        if (entity is not null)
        {
            context.Listenings.Remove(entity);
        }
    }

    public async Task<Listening?> GetByPracticeIdUserIdLanguageIdAsync(int practiceId, string userId, int languageId) =>
        await context.Listenings
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.PracticeId == practiceId && l.UserId == userId && l.LanguageId == languageId);
}
