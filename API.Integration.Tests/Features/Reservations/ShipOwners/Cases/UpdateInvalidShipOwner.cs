using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace ShipOwners {

    public class UpdateInvalidShipOwner : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Nationality_Must_Exist();
            yield return TaxOffice_Must_Exist();
            yield return ShipOwner_Must_Not_Be_Already_Updated();
        }

        private static object[] Nationality_Must_Exist() {
            return new object[] {
                new TestShipOwner {
                    StatusCode = 456,
                    Id = 1,
                    NationalityId = 9999,
                    TaxOfficeId = 1,
                    Description = Helpers.CreateRandomString(512),
                    VatNumber = Helpers.CreateRandomString(36),
                    Branch = 0,
                    PostalCode = Helpers.CreateRandomString(10),
                    City = Helpers.CreateRandomString(128),
                    PutAt = "2023-04-08 00:00:00"
                }
            };
        }

        private static object[] TaxOffice_Must_Exist() {
            return new object[] {
                new TestShipOwner {
                    StatusCode = 458,
                    Id = 1,
                    NationalityId = 1,
                    TaxOfficeId = 999,
                    Description = Helpers.CreateRandomString(512),
                    VatNumber = Helpers.CreateRandomString(36),
                    Branch = 0,
                    PostalCode = Helpers.CreateRandomString(10),
                    City = Helpers.CreateRandomString(128),
                    PutAt = "2023-04-08 00:00:00"
                }
            };
        }

        private static object[] ShipOwner_Must_Not_Be_Already_Updated() {
            return new object[] {
                new TestShipOwner {
                    StatusCode = 415,
                    Id = 1,
                    NationalityId = 1,
                    TaxOfficeId = 1,
                    Description = Helpers.CreateRandomString(512),
                    VatNumber = Helpers.CreateRandomString(36),
                    Branch = 0,
                    PostalCode = Helpers.CreateRandomString(10),
                    City = Helpers.CreateRandomString(128),
                    PutAt = "2023-09-07 09:57:05"
                }
            };
        }

    }

}
