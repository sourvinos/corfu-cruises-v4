using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace TaxOffices {

    public class CreateValidTaxOffice : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestTaxOffice {
                    Description = Helpers.CreateRandomString(128)
                }
            };
        }

    }

}
