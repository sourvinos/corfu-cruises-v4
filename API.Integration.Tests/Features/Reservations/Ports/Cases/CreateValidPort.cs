using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Ports {

    public class CreateValidPort : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestPort {
                    Abbreviation= Helpers.CreateRandomString(5),
                    Description = Helpers.CreateRandomString(128),
                    Locode = "GRXXX",
                    StopOrder = 5
                }
            };
        }

    }

}
