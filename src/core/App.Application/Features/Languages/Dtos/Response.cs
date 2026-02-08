namespace App.Application.Features.Languages.Dtos;

/// <summary>
/// RESPONSE DTO FOR LANGUAGE ENTITY.
/// </summary>
public record LanguageDto
(
    int Id,
    string Name,
    string? ImageUrl
);
