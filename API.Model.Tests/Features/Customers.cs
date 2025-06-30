using API.Features.Reservations.Customers;
using API.Model.Tests.Infrastructure;
using FluentValidation.TestHelper;
using Xunit;

namespace API.Model.Tests.Features.Customers {

    public class Customers {

        [Theory]
        [ClassData(typeof(ValidateStringNotEmpty))]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Description(string description) {
            new CustomerValidator()
                .TestValidate(new CustomerWriteDto { Description = description })
                .ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotEmpty))]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_FullDescription(string fullDescription) {
            new CustomerValidator()
                .TestValidate(new CustomerWriteDto { FullDescription = fullDescription })
                .ShouldHaveValidationErrorFor(x => x.FullDescription);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_VatNumber(string vatNumber) {
            new CustomerValidator()
               .TestValidate(new CustomerWriteDto { VatNumber = vatNumber })
               .ShouldHaveValidationErrorFor(x => x.VatNumber);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Branch(int branch) {
            new CustomerValidator()
               .TestValidate(new CustomerWriteDto { Branch = branch })
               .ShouldHaveValidationErrorFor(x => x.Branch);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Profession(string profession) {
            new CustomerValidator()
               .TestValidate(new CustomerWriteDto { Profession = profession })
               .ShouldHaveValidationErrorFor(x => x.Profession);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Street(string street) {
            new CustomerValidator()
                .TestValidate(new CustomerWriteDto { Street = street })
                .ShouldHaveValidationErrorFor(x => x.Street);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Number(string number) {
            new CustomerValidator()
                .TestValidate(new CustomerWriteDto { Number = number })
                .ShouldHaveValidationErrorFor(x => x.Number);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_PostalCode(string postalCode) {
            new CustomerValidator()
                .TestValidate(new CustomerWriteDto { PostalCode = postalCode })
                .ShouldHaveValidationErrorFor(x => x.PostalCode);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_City(string city) {
            new CustomerValidator()
                .TestValidate(new CustomerWriteDto { City = city })
                .ShouldHaveValidationErrorFor(x => x.City);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_PersonInCharge(string personInCharge) {
            new CustomerValidator()
                .TestValidate(new CustomerWriteDto { PersonInCharge = personInCharge })
                .ShouldHaveValidationErrorFor(x => x.PersonInCharge);
        }

        [Theory]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Phones(string phones) {
            new CustomerValidator()
               .TestValidate(new CustomerWriteDto { Phones = phones })
               .ShouldHaveValidationErrorFor(x => x.Phones);
        }

        [Theory]
        [ClassData(typeof(ValidateEmail))]
        public void Invalid_Email(string email) {
            new CustomerValidator()
               .TestValidate(new CustomerWriteDto { Email = email })
               .ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [ClassData(typeof(ValidateEmail))]
        public void Invalid_BalanceLimit(decimal balanceLimit) {
            new CustomerValidator()
               .TestValidate(new CustomerWriteDto { BalanceLimit = balanceLimit })
               .ShouldHaveValidationErrorFor(x => x.BalanceLimit);
        }

    }

}