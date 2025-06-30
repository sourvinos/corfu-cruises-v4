using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Ports {

    public class UpdateValidPort : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestPort {
                    Id = 1,
                    Abbreviation = Helpers.CreateRandomString(5),
                    Description = Helpers.CreateRandomString(128),
                    Locode = "GRXXX",
                    StopOrder = 1,
                    PutAt = "2024-01-19 07:45:38"
                }
            };
        }

    }

}
