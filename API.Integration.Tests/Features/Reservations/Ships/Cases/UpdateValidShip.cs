using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Ships {

    public class UpdateValidShip : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestShip {
                    Id = 1,
                    ShipOwnerId = 1,
                    Description = Helpers.CreateRandomString(15),
                    Abbreviation = Helpers.CreateRandomString(5),
                    PutAt= "2024-01-19 07:44:22"
                }
            };
        }

    }

}
