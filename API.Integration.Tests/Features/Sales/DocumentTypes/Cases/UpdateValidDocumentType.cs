using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace DocumentTypes {

    public class UpdateValidDocumentType : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestDocumentType {
                    Id = 2,
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
                    IsActive = true,
                    PutAt = "2024-01-05 09:38:02"
                }
            };
        }

    }

}
