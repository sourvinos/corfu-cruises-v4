using System;
using System.Collections;
using System.Collections.Generic;

namespace Reservations {

    public class ActiveSimpleUsersCanUpdateOwnedRecordsWhenValid : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Simple_Users_Can_Update_Own_Records_When_Valid();
        }

        private static object[] Simple_Users_Can_Update_Own_Records_When_Valid() {
            return new object[] {
                new TestUpdateReservation {
                    ReservationId = Guid.Parse("5bccac1d-caa4-47b6-869a-e63979c4103d"),
                    Date = "2023-07-01",
                    Now = new DateTime(2022, 3, 2, 11, 30, 00),
                    CustomerId = 2,
                    DestinationId = 1,
                    PickupPointId = 98,
                    PortId = 1,
                    RefNo = "PA74512",
                    TicketNo = "OUH74",
                    Adults = 3,
                    Kids = 0,
                    Free = 0,
                    Passengers = new List<TestPassenger>() {
                        new() { ReservationId = Guid.Parse("5bccac1d-caa4-47b6-869a-e63979c4103d"), Id = 64400, GenderId = 2, NationalityId = 243, Lastname = "KOYRI", Firstname = "MARILENA", Birthdate = "2022-05-05" },
                        new() { ReservationId = Guid.Parse("5bccac1d-caa4-47b6-869a-e63979c4103d"), Id = 64401, Lastname = "KOYRI", Firstname = "JANE", Birthdate = "2022-05-05", NationalityId = 243, GenderId = 2},
                        new() { ReservationId = Guid.Parse("5bccac1d-caa4-47b6-869a-e63979c4103d"), Id = 64402, Lastname = "NEW", Firstname = "JANE", Birthdate = "2022-05-05", NationalityId = 243, GenderId = 2 }
                    },
                    PutAt = "2023-09-14 05:17:50"
                }
            };
        }

    }

}