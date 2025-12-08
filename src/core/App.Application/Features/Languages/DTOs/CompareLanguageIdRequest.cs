namespace App.Application.Features.Languages.DTOs;

public record CompareLanguageIdRequest
{
    public string UserId { get; init; } = default!;
    public int LanguageId { get; init; }
}
