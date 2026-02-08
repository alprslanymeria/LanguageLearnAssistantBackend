using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LANGUAGE ENTITY.
/// </summary>
public class LanguageRepository(AppDbContext context) : ILanguageRepository
{
    public async Task AddAsync(Language entity) => await context.Languages.AddAsync(entity);

    public async Task<Language?> GetByIdAsync(int id) =>
        await context.Languages
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

    public void Update(Language entity) => context.Languages.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.Languages.FindAsync(id);

        if (entity is not null)
        {
            context.Languages.Remove(entity);
        }
    }

    public async Task<List<Language>> GetLanguagesAsync() =>
        await context.Languages
            .AsNoTracking()
            .ToListAsync();

    public async Task<Language?> ExistsByNameAsync(string name) =>
        await context.Languages
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Name == name);

    public async Task<Language?> GetByNameAsync(string name) =>
        await context.Languages
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Name == name);

    public async Task<List<Language>> GetAllAsync() =>
        await context.Languages
            .AsNoTracking()
            .ToListAsync();
}
