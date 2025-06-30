using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Drivers {

    public class UpdateValidDriver : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestDriver {
                    Id = 1,
                    Description = Helpers.CreateRandomString(128),
                    PutAt = "2024-01-19 07:44:25"
                }
            };
        }

    }

}
