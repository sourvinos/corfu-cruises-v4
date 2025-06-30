using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Ships {

    public class CreateValidShip : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestShip {
                    ShipOwnerId = 1,
                    Description = Helpers.CreateRandomString(15),
                    Abbreviation = Helpers.CreateRandomString(5)
                }
            };
        }

    }

}
