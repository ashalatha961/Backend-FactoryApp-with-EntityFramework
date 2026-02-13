using FluentValidation;
using PopsicleFactoryCo.Models;

namespace PopsicleFactoryCo.Validators
{
    //PopsicleValidator class inherits from AbstractValidator<T> (where T is your model) and
    //validation rules are defined in the constructor using the fluent interface
    public class PopsicleValidator : AbstractValidator<PopsicleModel>
    {
        public PopsicleValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id should always be number");
            RuleFor(x => x.Name)
                .Length(0, 100)
                .NotEmpty()
                .WithMessage("Name is required")
                .WithMessage("Name must not exceed 100 characters");
            RuleFor(x => x.Price).InclusiveBetween(0, 50);
            RuleFor(x => x.Flavour).NotEmpty();
            //https://docs.fluentvalidation.net/en/latest/aspnet.html
        }
    }

}