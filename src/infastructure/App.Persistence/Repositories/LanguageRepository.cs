using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LANGUAGE ENTITY.
/// </summary>
public class LanguageRepository(AppDbContext context) : ILanguageRepository
{

    public async Task<List<Language>> GetLanguagesAsync()
    {
        return await context.Languages
            .AsNoTracking()
            .ToListAsync();
    }

    // HELPER
    public async Task<Language?> ExistsByNameAsync(string name)
    {
        return await context.Languages
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Name == name);
    }

    public async Task CreateAsync(Language entity) => await context.Languages.AddAsync(entity);

    public Task<Language?> GetByIdAsync(int id) =>
        context.Languages
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

    public Language Update(Language entity)
    {
        context.Languages.Update(entity);

        return entity;
    }

    public void Delete(Language entity)
    {
        context.Languages
            .Remove(entity);
    }
}
