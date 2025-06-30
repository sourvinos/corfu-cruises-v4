using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace PaymentMethods {

    public class UpdateInvalidPaymentMethod : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return PaymentMethod_Must_Not_Be_Already_Updated();
        }

        private static object[] PaymentMethod_Must_Not_Be_Already_Updated(){
            return new object[] {
                new TestPaymentMethod {
                    StatusCode = 415,
                    Id = 4,
                    Description = Helpers.CreateRandomString(128),
                    PutAt = "2024-01-01 10:00:00"
                }
            };
        }

    }

}
