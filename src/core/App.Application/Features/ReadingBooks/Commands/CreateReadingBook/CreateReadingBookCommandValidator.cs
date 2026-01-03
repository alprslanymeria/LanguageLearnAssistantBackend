using FluentValidation;

namespace App.Application.Features.ReadingBooks.Commands.CreateReadingBook;

/// <summary>
/// VALIDATOR FOR CREATE READING BOOK COMMAND.
/// </summary>
public class CreateReadingBookCommandValidator : AbstractValidator<CreateReadingBookCommand>
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    private static readonly string[] AllowedSourceExtensions = [".pdf", ".epub", ".txt", ".docx", ".doc"];
    private const long MaxImageFileSize = 5 * 1024 * 1024; // 5MB
    private const long MaxSourceFileSize = 50 * 1024 * 1024; // 50MB

    public CreateReadingBookCommandValidator()
    {
        RuleFor(x => x.ReadingId)
            .GreaterThan(0)
            .WithMessage("READING ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("NAME IS REQUIRED")
            .MaximumLength(200)
            .WithMessage("NAME MUST NOT EXCEED 200 CHARACTERS");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.ImageFile)
            .NotNull()
            .WithMessage("IMAGE FILE IS REQUIRED")
            .Must(file => file is not null && file.Length > 0)
            .WithMessage("IMAGE FILE CANNOT BE EMPTY")
            .Must(file => file is null || file.Length <= MaxImageFileSize)
            .WithMessage($"IMAGE FILE SIZE MUST NOT EXCEED {MaxImageFileSize / 1024 / 1024}MB")
            .Must(file => file is null || AllowedImageExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
            .WithMessage($"IMAGE FILE MUST BE ONE OF THE FOLLOWING TYPES: {string.Join(", ", AllowedImageExtensions)}");

        RuleFor(x => x.SourceFile)
            .NotNull()
            .WithMessage("SOURCE FILE IS REQUIRED")
            .Must(file => file is not null && file.Length > 0)
            .WithMessage("SOURCE FILE CANNOT BE EMPTY")
            .Must(file => file is null || file.Length <= MaxSourceFileSize)
            .WithMessage($"SOURCE FILE SIZE MUST NOT EXCEED {MaxSourceFileSize / 1024 / 1024}MB")
            .Must(file => file is null || AllowedSourceExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
            .WithMessage($"SOURCE FILE MUST BE ONE OF THE FOLLOWING TYPES: {string.Join(", ", AllowedSourceExtensions)}");
    }
}
