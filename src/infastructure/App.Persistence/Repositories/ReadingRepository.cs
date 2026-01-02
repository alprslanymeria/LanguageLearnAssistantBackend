using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR READING ENTITY.
/// </summary>
public class ReadingRepository(AppDbContext context) : GenericRepository<Reading, int>(context), IReadingRepository
{

}
