using App.Application.Features.FlashcardCategories.Dtos;
using FluentValidation;

namespace App.Application.Features.FlashcardCategories.Validators;

/// <summary>
/// VALIDATOR FOR CREATE FLASHCARD CATEGORY REQUEST.
/// </summary>
public class CreateFlashcardCategoryRequestValidator : AbstractValidator<CreateFlashcardCategoryRequest>
{
    public CreateFlashcardCategoryRequestValidator()
    {
        RuleFor(x => x.FlashcardId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD ID MUST BE GREATER THAN 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("NAME IS REQUIRED")
            .MaximumLength(200)
            .WithMessage("NAME MUST NOT EXCEED 200 CHARACTERS.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");
    }
}

/// <summary>
/// VALIDATOR FOR UPDATE FLASHCARD CATEGORY REQUEST.
/// </summary>
public class UpdateFlashcardCategoryRequestValidator : AbstractValidator<UpdateFlashcardCategoryRequest>
{
    public UpdateFlashcardCategoryRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.FlashcardId)
            .GreaterThan(0)
            .WithMessage("FLASHCARD ID MUST BE GREATER THAN 0.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("NAME IS REQUIRED.")
            .MaximumLength(200)
            .WithMessage("NAME MUST NOT EXCEED 200 CHARACTERS.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");
    }
}
