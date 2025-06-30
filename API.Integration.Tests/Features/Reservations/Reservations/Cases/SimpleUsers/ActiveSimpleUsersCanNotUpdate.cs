using System;
using System.Collections;
using System.Collections.Generic;

namespace Reservations {

    public class ActiveSimpleUsersCanNotUpdate : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Simple_Users_Can_Not_Update_Not_Owned_Reservations();
            yield return Simple_Users_Can_Not_Update_Owned_Reservations_After_Departure();
        }

        private static object[] Simple_Users_Can_Not_Update_Not_Owned_Reservations() {
            return new object[] {
                new TestUpdateReservation {
                    StatusCode = 490,
                    ReservationId = Guid.Parse("08da3414-d27e-4393-867b-97c3c79d71d6"),
                    Date = "2022-06-11",
                    CustomerId = 1,
                    DestinationId = 2,
                    PickupPointId = 94,
                    ShipId = 2,
                    RefNo = "BL983",
                    TicketNo = "A12",
                    Adults = 2,
                    Kids = 1,
                    PutAt = "2023-09-14 05:17:50"
                }
            };
        }

        private static object[] Simple_Users_Can_Not_Update_Owned_Reservations_After_Departure() {
            return new object[] {
                new TestUpdateReservation {
                    StatusCode = 431,
                    ReservationId = Guid.Parse("08da2438-f893-40c4-8ead-5bc3ed9af591"),
                    Date = "2022-04-29",
                    Now = new DateTime(2022, 4, 29, 11, 30, 00),
                    CustomerId = 2,
                    DestinationId = 1,
                    PickupPointId = 215,
                    RefNo = "PA17",
                    TicketNo = "14",
                    Adults = 2,
                    PutAt = "2023-09-14 05:17:50"
               }
            };
        }

    }

}