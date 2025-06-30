using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace PaymentMethods {

    public class UpdateValidPaymentMethod : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestPaymentMethod {
                    Id = 4,
                    Description = Helpers.CreateRandomString(128),
                    PutAt = "2024-01-01 00:00:00"
                }
            };
        }

    }

}
