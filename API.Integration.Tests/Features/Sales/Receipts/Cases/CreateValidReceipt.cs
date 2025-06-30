using System.Collections;
using System.Collections.Generic;

namespace Receipts {

    public class CreateValidTransaction : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestReceipt {
                    CustomerId = 1,
                    DocumentTypeId = 4,
                    PaymentMethodId = 1,
                    ShipOwnerId = 1,
                    Date = "2024-02-10",
                    InvoiceNo = 1,
                    NetAmount = 12,
                    VatPercent = 24,
                    VatAmount = 2.88M,
                    GrossAmount = 14.88M
                }
            };
        }

    }

}
