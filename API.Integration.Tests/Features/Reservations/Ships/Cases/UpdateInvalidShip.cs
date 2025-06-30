using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Ships {

    public class UpdateInvalidShip : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ShipOwner_Must_Exist();
            yield return Ship_Must_Not_Be_Already_Updated();
        }

        private static object[] ShipOwner_Must_Exist() {
            return new object[] {
                new TestShip {
                    Id = 6,
                    StatusCode = 449,
                    ShipOwnerId = 9999,
                    Description = Helpers.CreateRandomString(15),
                    Abbreviation = Helpers.CreateRandomString(5)
                }
            };
        }

        private static object[] Ship_Must_Not_Be_Already_Updated() {
            return new object[] {
                new TestShip {
                    StatusCode = 415,
                    Id = 1,
                    ShipOwnerId = 1,
                    Description = Helpers.CreateRandomString(15),
                    Abbreviation = Helpers.CreateRandomString(5),
                    PutAt= "2023-09-07 09:57:41"
                }
            };
        }

    }

}
