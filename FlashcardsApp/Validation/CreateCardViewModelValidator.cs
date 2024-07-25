using FluentValidation;
using FlashcardsApp.Models;

namespace FlashcardsApp.Validation
{
    public class CreateCardViewModelValidator : AbstractValidator<CreateCardViewModel>
    {
        public CreateCardViewModelValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("Question is required.");

            RuleFor(x => x.Answer)
                .NotEmpty().WithMessage("Answer is required.");

            RuleFor(x => x.DeckId)
                .GreaterThan(0).WithMessage("Please select a valid deck.");

            RuleFor(x => x.ImageFile)
                .Must(x => x == null || (x.Length > 0 && (x.ContentType == "image/jpeg" || x.ContentType == "image/png")))
                .WithMessage("Please upload a valid image file (jpg, jpeg, png).");
        }
    }
}