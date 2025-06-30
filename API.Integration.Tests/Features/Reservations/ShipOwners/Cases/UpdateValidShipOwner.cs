using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace ShipOwners {

    public class UpdateValidShipOwner : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestShipOwner {
                    Id = 1,
                    NationalityId = 1,
                    TaxOfficeId = 1,
                    Description = Helpers.CreateRandomString(128),
                    VatNumber = Helpers.CreateRandomString(36),
                    Branch = 0,
                    PostalCode = Helpers.CreateRandomString(10),
                    City = Helpers.CreateRandomString(128),
                    PutAt = "2024-03-15 05:24:52"
                }
            };
        }

    }

}
