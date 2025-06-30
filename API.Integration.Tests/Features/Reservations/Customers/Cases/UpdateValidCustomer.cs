using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Customers {

    public class UpdateValidCustomer : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestCustomer {
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
                    PutAt = "2023-04-08 00:00:00"
                }
            };
        }

    }

}
