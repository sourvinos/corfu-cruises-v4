using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace ShipCrews {

    public class UpdateInvalidCrew : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Gender_Must_Exist();
            yield return Nationality_Must_Exist();
            yield return Ship_Must_Exist();
            yield return Specialty_Must_Exist();
            yield return Crew_Must_Not_Be_Already_Updated();
        }

        private static object[] Gender_Must_Exist() {
            return new object[] {
                new TestCrew {
                    StatusCode = 457,
                    Id = 22,
                    GenderId = 5,
                    NationalityId = 1,
                    ShipId = 1,
                    SpecialtyId = 1,
                    Lastname = Helpers.CreateRandomString(128),
                    Firstname = Helpers.CreateRandomString(128),
                    Birthdate = "1970-01-01",
                }
            };
        }

        private static object[] Nationality_Must_Exist() {
            return new object[] {
                new TestCrew {
                    StatusCode = 456,
                    Id = 22,
                    GenderId = 1,
                    NationalityId = 9999,
                    ShipId = 1,
                    SpecialtyId = 1,
                    Lastname = Helpers.CreateRandomString(128),
                    Firstname = Helpers.CreateRandomString(128),
                    Birthdate = "1970-01-01",
                }
            };
        }

        private static object[] Ship_Must_Exist() {
            return new object[] {
                new TestCrew {
                    StatusCode = 454,
                    Id = 22,
                    GenderId = 1,
                    NationalityId = 1,
                    ShipId = 9999,
                    SpecialtyId = 1,
                    Lastname = Helpers.CreateRandomString(128),
                    Firstname = Helpers.CreateRandomString(128),
                    Birthdate = "1970-01-01"
                }
            };
        }

        private static object[] Specialty_Must_Exist() {
            return new object[] {
                new TestCrew {
                    StatusCode = 464,
                    Id = 22,
                    GenderId = 1,
                    NationalityId = 1,
                    ShipId = 1,
                    SpecialtyId = 999,
                    Lastname = Helpers.CreateRandomString(128),
                    Firstname = Helpers.CreateRandomString(128),
                    Birthdate = "1970-01-01"
                }
            };
        }

        private static object[] Crew_Must_Not_Be_Already_Updated() {
            return new object[] {
                new TestCrew {
                    StatusCode = 415,
                    Id = 22,
                    GenderId = 1,
                    NationalityId = 1,
                    ShipId = 1,
                    SpecialtyId = 1,
                    Lastname = Helpers.CreateRandomString(128),
                    Firstname = Helpers.CreateRandomString(128),
                    Birthdate = "1970-01-01",
                    PutAt = "2023-09-07 09:54:22"
                }
            };
        }

    }

}
