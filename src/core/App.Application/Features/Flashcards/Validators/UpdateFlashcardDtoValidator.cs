using App.Application.Features.Flashcards.Dtos;
using FluentValidation;

namespace App.Application.Features.Flashcards.Validators;

public class UpdateFlashcardDtoValidator : AbstractValidator<UpdateFlashcardDto>
{
    public UpdateFlashcardDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Flashcard Id must be greater than 0");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required");

        RuleFor(x => x.LanguageId)
            .GreaterThan(0).WithMessage("Language Id must be greater than 0");

        RuleFor(x => x.PracticeId)
            .GreaterThan(0).WithMessage("Practice Id must be greater than 0");
    }
}
