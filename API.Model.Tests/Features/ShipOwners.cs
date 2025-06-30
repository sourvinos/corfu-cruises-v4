using API.Features.Reservations.ShipOwners;
using API.Model.Tests.Infrastructure;
using FluentValidation.TestHelper;
using Xunit;

namespace API.Model.Tests.Features.ShipOwners {

    public class ShipOwners {

        [Theory]
        [ClassData(typeof(ValidateStringNotEmpty))]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Description(string description) {
            new ShipOwnerValidator()
                .TestValidate(new ShipOwnerWriteDto { Description = description })
                .ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_VatNumber(string vatNumber) {
            new ShipOwnerValidator()
               .TestValidate(new ShipOwnerWriteDto { VatNumber = vatNumber })
               .ShouldHaveValidationErrorFor(x => x.VatNumber);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Profession(string profession) {
            new ShipOwnerValidator()
                .TestValidate(new ShipOwnerWriteDto { Profession = profession })
                .ShouldHaveValidationErrorFor(x => x.Profession);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Address(string street) {
            new ShipOwnerValidator()
                .TestValidate(new ShipOwnerWriteDto { Street = street })
                .ShouldHaveValidationErrorFor(x => x.Street);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_City(string city) {
            new ShipOwnerValidator()
                .TestValidate(new ShipOwnerWriteDto { City = city })
                .ShouldHaveValidationErrorFor(x => x.City);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Phones(string phones) {
            new ShipOwnerValidator()
                .TestValidate(new ShipOwnerWriteDto { Phones = phones })
                .ShouldHaveValidationErrorFor(x => x.Phones);
        }

        [Theory]
        [ClassData(typeof(ValidateEmail))]
        public void Invalid_Email(string email) {
            new ShipOwnerValidator()
                .TestValidate(new ShipOwnerWriteDto { Email = email })
                .ShouldHaveValidationErrorFor(x => x.Email);
        }

    }

}