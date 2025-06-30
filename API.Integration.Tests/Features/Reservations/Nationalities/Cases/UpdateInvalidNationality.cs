using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Nationalities {

    public class UpdateInvalidNationality : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Nationality_Must_Not_Be_Already_Updated();
        }

        private static object[] Nationality_Must_Not_Be_Already_Updated() {
            return new object[] {
                new TestNationality {
                    StatusCode = 415,
                    Id = 254,
                    Description = Helpers.CreateRandomString(128),
                    Code = Helpers.CreateRandomString(10),
                    PutAt = "2023-09-07 09:55:22"
                }
            };
        }

    }

}
