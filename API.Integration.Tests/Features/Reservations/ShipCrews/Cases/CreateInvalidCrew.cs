using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace ShipCrews {

    public class CreateInvalidCrew : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Gender_Must_Exist();
            yield return Gender_Must_Be_Active();
            yield return Nationality_Must_Exist();
            yield return Nationality_Must_Be_Active();
            yield return Ship_Must_Exist();
            yield return Ship_Must_Be_Active();
            yield return Specialty_Must_Exist();
            yield return Specialty_Must_Be_Active();
        }

        private static object[] Gender_Must_Exist() {
            return new object[] {
                new TestCrew {
                    StatusCode = 457,
                    GenderId = 5,
                    NationalityId = 1,
                    SpecialtyId = 1,
                    ShipId = 1,
                    Lastname = Helpers.CreateRandomString(128),
                    Firstname = Helpers.CreateRandomString(128),
                    Birthdate = "1970-01-01",
                }
            };
        }

        private static object[] Gender_Must_Be_Active() {
            return new object[] {
                new TestCrew {
                    StatusCode = 457,
                    GenderId = 4,
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

        private static object[] Nationality_Must_Be_Active() {
            return new object[] {
                new TestCrew {
                    StatusCode = 456,
                    GenderId = 1,
                    NationalityId = 254,
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

        private static object[] Ship_Must_Be_Active() {
            return new object[] {
                new TestCrew {
                    StatusCode = 454,
                    GenderId = 1,
                    NationalityId = 1,
                    ShipId = 7,
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

        private static object[] Specialty_Must_Be_Active() {
            return new object[] {
                new TestCrew {
                    StatusCode = 464,
                    GenderId = 1,
                    NationalityId = 1,
                    ShipId = 1,
                    SpecialtyId = 24,
                    Lastname = Helpers.CreateRandomString(128),
                    Firstname = Helpers.CreateRandomString(128),
                    Birthdate = "1970-01-01",
                }
            };
        }

    }

}
