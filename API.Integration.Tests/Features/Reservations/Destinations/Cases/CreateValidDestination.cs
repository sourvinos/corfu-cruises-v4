using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Destinations {

    public class CreateValidDestination : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestDestination {
                    Abbreviation = Helpers.CreateRandomString(5),
                    Description = Helpers.CreateRandomString(128)
                }
            };
        }

    }

}
