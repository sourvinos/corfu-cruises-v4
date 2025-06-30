using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace ShipCrews {

    public class UpdateValidCrew : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestCrew {
                    Id = 22,
                    GenderId = 1,
                    NationalityId = 1,
                    ShipId = 1,
                    SpecialtyId = 1,
                    Lastname = Helpers.CreateRandomString(128),
                    Firstname = Helpers.CreateRandomString(128),
                    Birthdate = "1970-01-01",
                    PutAt = "2023-09-14 05:17:56"
                }
            };
        }

    }

}
