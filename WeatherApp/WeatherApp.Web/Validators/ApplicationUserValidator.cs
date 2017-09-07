using FluentValidation;
using WeatherApp.Web.Models.AccountViewModels;

namespace WeatherApp.Web.Validators
{
    public class ApplicationUserValidator : AbstractValidator<RegisterViewModel>
    {
        public ApplicationUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().Length(6, 17);
            RuleFor(x => x.ConfirmPassword).NotEmpty().Length(6, 17).Equal(x => x.Password);
        }
    }
}
