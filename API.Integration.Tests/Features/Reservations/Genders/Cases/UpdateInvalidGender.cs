using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Genders {

    public class UpdateInvalidGender : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Gender_Must_Not_Be_Already_Updated();
        }

        private static object[] Gender_Must_Not_Be_Already_Updated() {
            return new object[] {
                new TestGender {
                    StatusCode = 415,
                    Id = 1,
                    Description = Helpers.CreateRandomString(128),
                    PutAt = "2023-09-14 05:17:45"
                }
            };
        }

    }

}
