using API.Infrastructure.Helpers;
using FluentValidation;

namespace API.Infrastructure.Users {

    public class UserNewValidator : AbstractValidator<UserNewDto> {

        public UserNewValidator() {
            RuleFor(x => x.Username).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Displayname).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Email).Must(EmailHelpers.BeValidEmailAddress).MaximumLength(128);
        }

    }

}