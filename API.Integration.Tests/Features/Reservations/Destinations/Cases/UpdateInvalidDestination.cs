using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Destinations {

    public class UpdateInvalidDestination : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Destination_Must_Not_Be_Already_Updated();
        }

        private static object[] Destination_Must_Not_Be_Already_Updated() {
            return new object[] {
                new TestDestination {
                    StatusCode = 415,
                    Id = 1,
                    Abbreviation = Helpers.CreateRandomString(5),
                    Description = Helpers.CreateRandomString(128),
                    PutAt = "2023-09-07 09:55:22"
                }
            };
        }

    }

}
