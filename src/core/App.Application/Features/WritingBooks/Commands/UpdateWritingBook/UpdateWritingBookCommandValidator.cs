using FluentValidation;

namespace App.Application.Features.WritingBooks.Commands.UpdateWritingBook;

/// <summary>
/// VALIDATOR FOR UPDATE WRITING BOOK COMMAND.
/// </summary>
public class UpdateWritingBookCommandValidator : AbstractValidator<UpdateWritingBookCommand>
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    private static readonly string[] AllowedSourceExtensions = [".pdf", ".epub", ".txt", ".docx", ".doc"];
    private const long MaxImageFileSize = 5 * 1024 * 1024; // 5MB
    private const long MaxSourceFileSize = 50 * 1024 * 1024; // 50MB

    public UpdateWritingBookCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0");

        RuleFor(x => x.WritingId)
            .GreaterThan(0)
            .WithMessage("WRITING ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("NAME IS REQUIRED")
            .MaximumLength(200)
            .WithMessage("NAME MUST NOT EXCEED 200 CHARACTERS");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        // IMAGE FILE IS OPTIONAL FOR UPDATE
        When(x => x.ImageFile is not null, () =>
        {
            RuleFor(x => x.ImageFile)
                .Must(file => file is null || file.Length <= MaxImageFileSize)
                .WithMessage($"IMAGE FILE SIZE MUST NOT EXCEED {MaxImageFileSize / 1024 / 1024}MB")
                .Must(file => file is null || AllowedImageExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                .WithMessage($"IMAGE FILE MUST BE ONE OF THE FOLLOWING TYPES: {string.Join(", ", AllowedImageExtensions)}");
        });

        // SOURCE FILE IS OPTIONAL FOR UPDATE
        When(x => x.SourceFile is not null, () =>
        {
            RuleFor(x => x.SourceFile)
                .Must(file => file is null || file.Length <= MaxSourceFileSize)
                .WithMessage($"SOURCE FILE SIZE MUST NOT EXCEED {MaxSourceFileSize / 1024 / 1024}MB")
                .Must(file => file is null || AllowedSourceExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                .WithMessage($"SOURCE FILE MUST BE ONE OF THE FOLLOWING TYPES: {string.Join(", ", AllowedSourceExtensions)}");
        });
    }
}
