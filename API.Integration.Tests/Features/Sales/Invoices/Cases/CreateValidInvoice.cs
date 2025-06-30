using System.Collections;
using System.Collections.Generic;

namespace Invoices {

    public class CreateValidTransaction : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidInvoice();
        }

        private static object[] ValidInvoice() {
            return new object[] {
                new TestInvoice {
                    CustomerId = 1,
                    DestinationId = 1,
                    DocumentTypeId = 1,
                    PaymentMethodId = 1,
                    ShipId = 1,
                    Date = "2024-02-10",
                    TripDate = "2024-02-10",
                    InvoiceNo = 1,
                    NetAmount = 12,
                    VatPercent = 24,
                    VatAmount = 2.88M,
                    GrossAmount = 14.88M,
                    PreviousBalance = 0,
                    NewBalance = 0
                }
            };
        }

    }

}
