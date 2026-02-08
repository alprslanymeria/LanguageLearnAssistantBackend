using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR PRACTICE ENTITY.
/// </summary>
public class PracticeRepository(AppDbContext context) : IPracticeRepository
{
    public async Task AddAsync(Practice entity) => await context.Practices.AddAsync(entity);

    public async Task<Practice?> GetByIdAsync(int id) =>
        await context.Practices
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

    public void Update(Practice entity) => context.Practices.Update(entity);

    public async Task RemoveAsync(int id)
    {
        var entity = await context.Practices.FindAsync(id);

        if (entity is not null)
        {
            context.Practices.Remove(entity);
        }
    }

    public async Task<List<Practice>> GetPracticesByLanguageAsync(string language) =>
        await context.Practices
            .AsNoTracking()
            .Where(p => p.Language.Name == language)
            .ToListAsync();

    public async Task<Practice?> GetPracticeByLanguageIdAndNameAsync(int languageId, string name) =>
        await context.Practices
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.LanguageId == languageId && p.Name == name);

    public async Task<Practice?> ExistsByLanguageIdAsync(int languageId) =>
        await context.Practices
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.LanguageId == languageId);

    public async Task<Practice?> ExistsByNameAndLanguageIdAsync(string name, int languageId) =>
        await context.Practices
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == name && p.LanguageId == languageId);
}
