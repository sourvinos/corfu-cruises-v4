using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Nationalities {

    public class UpdateValidNationality : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestNationality {
                    Id = 254,
                    Description = Helpers.CreateRandomString(128),
                    Code = Helpers.CreateRandomString(10),
                    PutAt = "2023-09-14 05:17:46"
                }
            };
        }

    }

}
