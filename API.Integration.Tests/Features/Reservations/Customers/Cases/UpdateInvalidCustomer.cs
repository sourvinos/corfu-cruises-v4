using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Customers {

    public class UpdateInvalidCustomer : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Nationality_Must_Exist();
            yield return TaxOffice_Must_Exist();
            yield return VatRegime_Must_Exist();
            yield return Customer_Must_Not_Be_Already_Updated();
        }

        private static object[] Nationality_Must_Exist() {
            return new object[] {
                new TestCustomer {
                    StatusCode = 456,
                    Id = 1,
                    NationalityId = 9999,
                    TaxOfficeId = 1,
                    Description = Helpers.CreateRandomString(128),
                    FullDescription = Helpers.CreateRandomString(512),
                    VatNumber = Helpers.CreateRandomString(36),
                    Branch = 0,
                    PostalCode = Helpers.CreateRandomString(10),
                    City = Helpers.CreateRandomString(128),
                    BalanceLimit = 0M,
                    PutAt = "2023-09-07 09:52:22"
                }
            };
        }

        private static object[] TaxOffice_Must_Exist() {
            return new object[] {
                new TestCustomer {
                    StatusCode = 458,
                    Id = 1,
                    NationalityId = 1,
                    TaxOfficeId = 999,
                    Description = Helpers.CreateRandomString(128),
                    FullDescription = Helpers.CreateRandomString(512),
                    VatNumber = Helpers.CreateRandomString(36),
                    Branch = 0,
                    PostalCode = Helpers.CreateRandomString(10),
                    City = Helpers.CreateRandomString(128),
                    BalanceLimit = 0M,
                    PutAt = "2023-09-07 09:52:22"
                }
            };
        }

        private static object[] VatRegime_Must_Exist() {
            return new object[]{
                new TestCustomer {
                    StatusCode = 463,
                    Id = 1,
                    NationalityId = 1,
                    TaxOfficeId = 1,
                    Description = Helpers.CreateRandomString(128),
                    FullDescription = Helpers.CreateRandomString(512),
                    VatNumber = Helpers.CreateRandomString(36),
                    Branch = 0,
                    PostalCode = Helpers.CreateRandomString(10),
                    City = Helpers.CreateRandomString(128),
                    BalanceLimit = 0M,
                    PutAt = "2023-09-07 09:52:22"
                }
            };
        }

        private static object[] Customer_Must_Not_Be_Already_Updated() {
            return new object[] {
                new TestCustomer {
                    StatusCode = 415,
                    Id = 1,
                    NationalityId = 1,
                    TaxOfficeId = 1,
                    Description = Helpers.CreateRandomString(128),
                    FullDescription = Helpers.CreateRandomString(512),
                    VatNumber = Helpers.CreateRandomString(36),
                    Branch = 0,
                    PostalCode = Helpers.CreateRandomString(10),
                    City = Helpers.CreateRandomString(128),
                    BalanceLimit = 0M,
                    PutAt = "2023-09-07 09:55:22"
                }
            };
        }

    }

}
