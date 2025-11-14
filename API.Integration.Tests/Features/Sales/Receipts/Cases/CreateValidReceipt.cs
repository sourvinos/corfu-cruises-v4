using System.Collections;
using System.Collections.Generic;
using API.Infrastructure.Helpers;

namespace Receipts {

    public class CreateValidTransaction : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestReceipt {
                    Date = DateHelpers.DateToISOString(DateHelpers.GetLocalDateTime()),
                    InvoiceNo = 1,
                    CustomerId = 1,
                    DocumentTypeId = 4,
                    PaymentMethodId = 1,
                    ShipOwnerId = 1,
                    NetAmount = 12,
                    VatPercent = 0,
                    VatAmount = 0M,
                    GrossAmount = 12M
                }
            };
        }

    }

}
