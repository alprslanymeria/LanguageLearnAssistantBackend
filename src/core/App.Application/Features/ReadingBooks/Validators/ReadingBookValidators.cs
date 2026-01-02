using App.Application.Features.ReadingBooks.Dtos;
using FluentValidation;

namespace App.Application.Features.ReadingBooks.Validators;

/// <summary>
/// VALIDATOR FOR CREATE READING BOOK REQUEST.
/// </summary>
public class CreateReadingBookRequestValidator : AbstractValidator<CreateReadingBookRequest>
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    private static readonly string[] AllowedSourceExtensions = [".pdf", ".epub", ".txt", ".docx", ".doc"];
    private const long MaxImageFileSize = 5 * 1024 * 1024; // 5MB
    private const long MaxSourceFileSize = 50 * 1024 * 1024; // 50MB

    public CreateReadingBookRequestValidator()
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

/// <summary>
/// VALIDATOR FOR UPDATE READING BOOK REQUEST.
/// </summary>
public class UpdateReadingBookRequestValidator : AbstractValidator<UpdateReadingBookRequest>
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    private static readonly string[] AllowedSourceExtensions = [".pdf", ".epub", ".txt", ".docx", ".doc"];
    private const long MaxImageFileSize = 5 * 1024 * 1024; // 5MB
    private const long MaxSourceFileSize = 50 * 1024 * 1024; // 50MB

    public UpdateReadingBookRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0");

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
