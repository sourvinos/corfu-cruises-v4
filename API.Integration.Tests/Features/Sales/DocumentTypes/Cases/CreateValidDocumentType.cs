using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace DocumentTypes {

    public class CreateValidDocumentType : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecordForInvoices();
            yield return ValidRecordForReceipts();
        }

        private static object[] ValidRecordForInvoices() {
            return new object[] {
                new TestDocumentType {
                    ShipId = 1,
                    ShipOwnerId = 1,
                    Abbreviation = Helpers.CreateRandomString(5),
                    Description = Helpers.CreateRandomString(128),
                    Batch = Helpers.CreateRandomString(5),
                    LastDate = "1970-01-01",
                    LastNo = 1,
                    Customers = "+",
                    Suppliers = "",
                    DiscriminatorId = 1,
                    IsMyData = true,
                    Table8_1 = "Table8_1",
                    Table8_8 = "Table8_8",
                    Table8_9 = "Table8_9",
                    IsActive = true
                }
            };
        }

        private static object[] ValidRecordForReceipts() {
            return new object[] {
                new TestDocumentType {
                    ShipId = 1,
                    ShipOwnerId = 1,
                    Abbreviation = Helpers.CreateRandomString(5),
                    Description = Helpers.CreateRandomString(128),
                    Batch = Helpers.CreateRandomString(5),
                    LastDate = "1970-01-01",
                    LastNo = 1,
                    Customers = "-",
                    Suppliers = "",
                    DiscriminatorId = 2,
                    IsMyData = false,
                    Table8_1 = "",
                    Table8_8 = "",
                    Table8_9 = "",
                    IsActive = true
                }
            };
        }

    }

}
