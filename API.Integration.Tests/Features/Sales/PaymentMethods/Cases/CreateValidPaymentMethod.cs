using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace PaymentMethods {

    public class CreateValidPaymentMethod : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestPaymentMethod {
                    Description = Helpers.CreateRandomString(128)
                }
            };
        }

    }

}
