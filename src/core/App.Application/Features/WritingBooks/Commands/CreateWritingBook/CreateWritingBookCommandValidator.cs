using FluentValidation;

namespace App.Application.Features.WritingBooks.Commands.CreateWritingBook;

/// <summary>
/// VALIDATOR FOR CREATE WRITING BOOK COMMAND.
/// </summary>
public class CreateWritingBookCommandValidator : AbstractValidator<CreateWritingBookCommand>
{
    private static readonly string[] _allowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    private static readonly string[] _allowedSourceExtensions = [".pdf", ".epub", ".txt", ".docx", ".doc"];
    private const long MaxImageFileSize = 5 * 1024 * 1024; // 5MB
    private const long MaxSourceFileSize = 50 * 1024 * 1024; // 50MB

    public CreateWritingBookCommandValidator()
    {

        RuleFor(x => x.Request.BookName)
            .NotEmpty()
            .WithMessage("NAME IS REQUIRED")
            .MaximumLength(200)
            .WithMessage("NAME MUST NOT EXCEED 200 CHARACTERS");

        RuleFor(x => x.Request.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.Request.ImageFile)
            .NotNull()
            .WithMessage("IMAGE FILE IS REQUIRED")
            .Must(file => file is not null && file.Length > 0)
            .WithMessage("IMAGE FILE CANNOT BE EMPTY")
            .Must(file => file is null || file.Length <= MaxImageFileSize)
            .WithMessage($"IMAGE FILE SIZE MUST NOT EXCEED {MaxImageFileSize / 1024 / 1024}MB")
            .Must(file => file is null || _allowedImageExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
            .WithMessage($"IMAGE FILE MUST BE ONE OF THE FOLLOWING TYPES: {string.Join(", ", _allowedImageExtensions)}");

        RuleFor(x => x.Request.SourceFile)
            .NotNull()
            .WithMessage("SOURCE FILE IS REQUIRED")
            .Must(file => file is not null && file.Length > 0)
            .WithMessage("SOURCE FILE CANNOT BE EMPTY")
            .Must(file => file is null || file.Length <= MaxSourceFileSize)
            .WithMessage($"SOURCE FILE SIZE MUST NOT EXCEED {MaxSourceFileSize / 1024 / 1024}MB")
            .Must(file => file is null || _allowedSourceExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
            .WithMessage($"SOURCE FILE MUST BE ONE OF THE FOLLOWING TYPES: {string.Join(", ", _allowedSourceExtensions)}");
    }
}
