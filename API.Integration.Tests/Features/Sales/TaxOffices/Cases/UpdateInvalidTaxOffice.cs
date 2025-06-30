using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace TaxOffices {

    public class UpdateInvalidTaxOffice : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return TaxOffice_Must_Not_Be_Already_Updated();
        }

        private static object[] TaxOffice_Must_Not_Be_Already_Updated(){
            return new object[] {
                new TestTaxOffice {
                    StatusCode = 415,
                    Id = 1,
                    Description = Helpers.CreateRandomString(128),
                    PutAt = "2023-09-07 09:55:22"
                }
            };
        }

    }

}
