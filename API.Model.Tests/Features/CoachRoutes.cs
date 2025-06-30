using API.Features.Reservations.CoachRoutes;
using API.Model.Tests.Infrastructure;
using FluentValidation.TestHelper;
using Xunit;

namespace API.Model.Tests.Features.CoachRoutes {

    public class Routes {

        [Theory]
        [ClassData(typeof(ValidateStringNotEmpty))]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Description(string description) {
            new CoachRouteValidator()
                .TestValidate(new CoachRouteWriteDto { Description = description })
                .ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotEmpty))]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Abbreviation(string abbreviation) {
            new CoachRouteValidator()
                .TestValidate(new CoachRouteWriteDto { Abbreviation = abbreviation })
                .ShouldHaveValidationErrorFor(x => x.Abbreviation);
        }

    }

}