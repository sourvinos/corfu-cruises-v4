using API.Infrastructure.Helpers;
using FluentValidation;

namespace API.Infrastructure.Account {

    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequestVM> {

        public ForgotPasswordValidator() {
            RuleFor(x => x.Email).NotEmpty().Must(EmailHelpers.BeValidEmailAddress);
        }

    }

}