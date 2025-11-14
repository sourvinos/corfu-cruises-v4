using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace ShipOwners {

    public class CreateValidShipOwner : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestShipOwner {
                    NationalityId = 1,
                    TaxOfficeId = 1,
                    VatPercent = 13,
                    VatPercentId = 1,
                    VatExemptionId = 0,
                    Description = Helpers.CreateRandomString(128),
                    DescriptionEn = Helpers.CreateRandomString(128),
                    VatNumber = Helpers.CreateRandomString(36),
                    Branch = 0,
                    PostalCode = Helpers.CreateRandomString(10),
                    City = Helpers.CreateRandomString(128)
                }
            };
        }

    }

}
