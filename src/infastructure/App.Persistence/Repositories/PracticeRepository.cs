using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR PRACTICE ENTITY.
/// </summary>
public class PracticeRepository(AppDbContext context) : IPracticeRepository
{

    public async Task<List<Practice>> GetPracticesByLanguageAsync(string language)
    {
        return await context.Practices
            .AsNoTracking()
            .Where(p => p.Language.Name == language)
            .ToListAsync();
    }


    // HELPER
    public async Task<Practice?> ExistsByLanguageIdAsync(int languageId)
    {
        return await context.Practices
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.LanguageId == languageId);
    }

    public async Task<Practice?> ExistsByNameAndLanguageIdAsync(string name, int languageId)
    {
        return await context.Practices
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == name && p.LanguageId == languageId);
    }

    public async Task CreateAsync(Practice entity) => await context.Practices.AddAsync(entity);

    public Task<Practice?> GetByIdAsync(int id) =>
        context.Practices
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

    public Practice Update(Practice entity)
    {
        context.Practices.Update(entity);

        return entity;
    }

    public void Delete(Practice entity)
    {
        context.Practices
            .Remove(entity);
    }
}
