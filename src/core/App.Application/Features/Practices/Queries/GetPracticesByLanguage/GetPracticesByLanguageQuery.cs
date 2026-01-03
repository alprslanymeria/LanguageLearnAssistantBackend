using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.Practices.Dtos;

namespace App.Application.Features.Practices.Queries.GetPracticesByLanguage;

/// <summary>
/// QUERY FOR RETRIEVING PRACTICES BY SPECIFIED LANGUAGE.
/// </summary>
public record GetPracticesByLanguageQuery(string Language) : IQuery<ServiceResult<List<PracticeDto>>>;
