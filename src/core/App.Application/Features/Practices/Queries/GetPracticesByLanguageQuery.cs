using App.Application.Common.CQRS;
using App.Application.Features.Practices.Dtos;

namespace App.Application.Features.Practices.Queries;

/// <summary>
/// QUERY TO GET PRACTICES BY LANGUAGE.
/// </summary>
public record GetPracticesByLanguageQuery(string Language) : IQuery<List<PracticeDto>>;
