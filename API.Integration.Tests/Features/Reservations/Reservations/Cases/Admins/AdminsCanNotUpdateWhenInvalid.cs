using System;
using System.Collections;
using System.Collections.Generic;

namespace Reservations {

    public class ActiveAdminsCanNotUpdateWhenInvalid : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Duplicate_Records_Are_Not_Allowed();
            yield return Nothing_For_This_Day();
            yield return Nothing_For_This_Day_And_Destination();
            yield return Nothing_For_This_Day_And_Destination_And_Port();
            yield return Passenger_Count_Is_Not_Correct();
            yield return Customer_Must_Exist();
            yield return Destination_Must_Exist();
            yield return Driver_Must_Exist();
            yield return Gender_Must_Exist();
            yield return Nationality_Must_Exist();
            yield return PickupPoint_Must_Exist();
            yield return PortAlternate_Must_Exist();
            yield return Ship_Must_Exist();
            yield return Reservation_Must_Not_Be_Already_Updated();
        }

        private static object[] Nothing_For_This_Day() {
            return new object[] {
                new TestUpdateReservation {
                    StatusCode = 410,
                    ReservationId = Guid.Parse("08da3262-6a70-4d76-88ca-3af058930134"),
                    CustomerId = 38,
                    DestinationId = 1,
                    DriverId = null,
                    PickupPointId = 133,
                    PortId = 1,
                    ShipId = null,
                    Date = "2022-01-01",
                    RefNo = "PA776",
                    TicketNo = "D5",
                    Adults = 2,
                    Passengers = new List<TestPassenger>() {
                        new() { Lastname = "TIMMINS", Firstname = "JOAN", Birthdate = "2065-08-21", NationalityId = 70, GenderId = 2 },
                        new() { Lastname = "TIMMINS", Firstname = "ALAN", Birthdate = "2065-07-17", NationalityId = 70, GenderId = 1 },
                    },
                    PutAt = "2023-09-14 05:17:50"
                }
            };
        }

        private static object[] Nothing_For_This_Day_And_Destination() {
            return new object[] {
                new TestUpdateReservation {
                    StatusCode = 410,
                    ReservationId = Guid.Parse("08da3262-6a70-4d76-88ca-3af058930134"),
                    CustomerId = 38,
                    DestinationId = 3,
                    DriverId = null,
                    PickupPointId = 133,
                    PortId = 1,
                    ShipId = null,
                    Date = "2022-05-01",
                    RefNo = "PA776",
                    TicketNo = "D5",
                    Adults = 2,
                    Passengers = new List<TestPassenger>() {
                        new() { Lastname = "TIMMINS", Firstname = "JOAN", Birthdate = "2065-08-21", NationalityId = 70, GenderId = 2 },
                        new() { Lastname = "TIMMINS", Firstname = "ALAN", Birthdate = "2065-07-17", NationalityId = 70, GenderId = 1 },
                    },
                    PutAt = "2023-09-14 05:17:50"
                }
            };
        }

        private static object[] Nothing_For_This_Day_And_Destination_And_Port() {
            return new object[] {
                new TestUpdateReservation {
                    StatusCode = 410,
                    ReservationId = Guid.Parse("08da2863-15d9-4338-81fa-637a52371163"),
                    CustomerId = 12,
                    DestinationId = 1,
                    DriverId = null,
                    PickupPointId = 266,
                    PortId = 2,
                    ShipId = 6,
                    Date = "2022-03-01",
                    RefNo = "PA6",
                    TicketNo = "Eagle Travel",
                    Adults = 3,
                    Passengers = new List<TestPassenger>() {
                        new() { Lastname = "sacomono", Firstname = "MARIECLEO", Birthdate = "1981-08-14", NationalityId = 70, GenderId = 2 },
                        new() { Lastname = "KAGREN", Firstname = "BIRCH", Birthdate = "1957-12-13", NationalityId = 70, GenderId = 2 },
                        new() { Lastname = "ANDREW", Firstname = "SUZAN", Birthdate = "1975-08-21", NationalityId = 70, GenderId = 2 }
                    },
                    PutAt = "2023-09-14 05:17:50"
                }
            };
        }

        private static object[] Duplicate_Records_Are_Not_Allowed() {
            return new object[] {
                new TestUpdateReservation {
                    StatusCode = 409,
                    ReservationId = Guid.Parse("08da28f1-0d1f-4cc5-851b-0832668cc79a"),
                    Date = "2022-05-01",
                    CustomerId = 2,
                    DestinationId = 1,
                    PickupPointId = 215,
                    PortId = 2,
                    ShipId = 6,
                    RefNo = "PA175",
                    TicketNo = "21",
                    PutAt = "2023-09-07 09:55:16"
                }
            };
        }

        private static object[] Passenger_Count_Is_Not_Correct() {
            return new object[]{
                new TestUpdateReservation{
                    StatusCode = 455,
                    ReservationId = Guid.Parse("08da2863-15d9-4338-81fa-637a52371163"),
                    CustomerId = 2,
                    DestinationId = 1,
                    PickupPointId = 215,
                    PortId = 2,
                    ShipId = 6,
                    RefNo = "PA175",
                    Date = "2022-05-01",
                    TicketNo = "21",
                    Adults = 2,
                    Passengers = new List<TestPassenger>() {
                        new() { Lastname = "AEDAN", Firstname = "ZAYAS", Birthdate = "1992-06-12", NationalityId = 123, GenderId = 1 },
                        new() { Lastname = "ALONA", Firstname = "CUTLER", Birthdate = "1964-04-28", NationalityId = 127, GenderId = 2 },
                        new() { Lastname = "LYA", Firstname = "TROWBRIDGE", Birthdate = "2015-01-21", NationalityId = 211, GenderId = 1 },
                    },
                    PutAt = "2023-09-07 09:55:16"
                }
            };
        }

        private static object[] Customer_Must_Exist() {
            return new object[] {
                new TestUpdateReservation {
                    StatusCode = 450,
                    ReservationId = Guid.Parse("08da2863-15d9-4338-81fa-637a52371163"),
                    CustomerId = 999,
                    DestinationId = 1,
                    PickupPointId = 215,
                    PortId = 2,
                    RefNo = "PA175",
                    Date = "2022-05-01",
                    TicketNo = "21",
                    Adults = 2,
                    PutAt = "2023-09-07 09:55:16"
                }
            };
        }

        private static object[] Destination_Must_Exist() {
            return new object[] {
                new TestUpdateReservation {
                    StatusCode = 451,
                    ReservationId = Guid.Parse("08da2863-15d9-4338-81fa-637a52371163"),
                    CustomerId = 2,
                    DestinationId = 99,
                    DriverId = 18,
                    PickupPointId = 215,
                    PortId = 2,
                    ShipId = 6,
                    Date = "2022-05-01",
                    RefNo = "PA175",
                    TicketNo = "21",
                    Adults = 2,
                    PutAt = "2023-09-07 09:55:16"
                }
            };
        }

        private static object[] Driver_Must_Exist() {
            return new object[] {
                new TestUpdateReservation {
                    StatusCode = 453,
                    ReservationId = Guid.Parse("08da2863-15d9-4338-81fa-637a52371163"),
                    CustomerId = 2,
                    DestinationId = 1,
                    DriverId = 99,
                    PickupPointId = 215,
                    PortId = 2,
                    ShipId = 6,
                    Date = "2022-05-01",
                    RefNo = "PA175",
                    TicketNo = "21",
                    Adults = 2,
                    PutAt = "2023-09-07 09:55:16"
                }
            };
        }

        private static object[] Gender_Must_Exist() {
            return new object[] {
                new TestUpdateReservation {
                    StatusCode = 457,
                    ReservationId = Guid.Parse("08da2863-15d9-4338-81fa-637a52371163"),
                    CustomerId = 2,
                    DestinationId = 1,
                    DriverId = 18,
                    PickupPointId = 215,
                    PortId = 2,
                    ShipId = 6,
                    Date = "2022-05-01",
                    RefNo = "PA175",
                    TicketNo = "21",
                    Adults = 2,
                    Free = 1,
                    Passengers = new List<TestPassenger>() {
                        new() { Lastname = "VOYER", Firstname = "ALESCANDRA", Birthdate = "1991-06-05", NationalityId = 233, GenderId = 99 },
                        new() { Lastname = "VOYER", Firstname = "ANNIE", Birthdate = "1962-12-25", NationalityId = 233, GenderId = 99 },
                        new() { Lastname = "VOYER", Firstname = "NATHAN", Birthdate = "2018-02-05", NationalityId = 233, GenderId = 99 }
                    },
                    PutAt = "2023-09-07 09:55:16"
                }
            };
        }

        private static object[] Nationality_Must_Exist() {
            return new object[]{
                new TestUpdateReservation{
                    StatusCode = 456,
                    ReservationId = Guid.Parse("08da2863-15d9-4338-81fa-637a52371163"),
                    CustomerId = 2,
                    DestinationId = 1,
                    DriverId = 18,
                    PickupPointId = 215,
                    PortId = 2,
                    ShipId = 6,
                    Date = "2022-05-01",
                    RefNo = "PA175",
                    TicketNo = "21",
                    Adults = 2,
                    Free = 1,
                    Passengers = new List<TestPassenger>() {
                        new() { Lastname = "VOYER", Firstname = "ALESCANDRA", Birthdate = "1991-06-05", NationalityId = 999, GenderId = 2 },
                        new() { Lastname = "VOYER", Firstname = "ANNIE", Birthdate = "1962-12-25", NationalityId = 999, GenderId = 2 },
                        new() { Lastname = "VOYER", Firstname = "NATHAN", Birthdate = "2018-02-05", NationalityId = 999, GenderId = 1 }
                    },
                    PutAt = "2023-09-07 09:55:16"
                }
            };
        }

        private static object[] PickupPoint_Must_Exist() {
            return new object[] {
                new TestUpdateReservation{
                    StatusCode = 452,
                    ReservationId = Guid.Parse("08da2863-15d9-4338-81fa-637a52371163"),
                    CustomerId = 2,
                    DestinationId = 1,
                    DriverId = 18,
                    PickupPointId = 999,
                    PortId = 2,
                    ShipId = 6,
                    Date = "2022-05-01",
                    TicketNo = "21",
                    RefNo = "PA175",
                    Adults = 2,
                    Free = 1,
                    PutAt = "2023-09-07 09:55:16"
                }
            };
        }

        private static object[] PortAlternate_Must_Exist() {
            return new object[] {
                new TestUpdateReservation{
                    StatusCode = 460,
                    ReservationId = Guid.Parse("08da2863-15d9-4338-81fa-637a52371163"),
                    CustomerId = 2,
                    DestinationId = 1,
                    DriverId = null,
                    PickupPointId = 215,
                    PortAlternateId = 999,
                    ShipId = null,
                    Date = "2022-05-01",
                    TicketNo = "21",
                    RefNo = "PA175",
                    Adults = 2,
                    Free = 1,
                    PutAt = "2023-09-07 09:55:16"
                }
            };
        }

        private static object[] Ship_Must_Exist() {
            return new object[] {
                new TestUpdateReservation{
                    StatusCode = 454,
                    ReservationId = Guid.Parse("08da2863-15d9-4338-81fa-637a52371163"),
                    CustomerId = 2,
                    DestinationId = 1,
                    DriverId = 18,
                    PickupPointId = 215,
                    PortId = 2,
                    ShipId = 99,
                    Date = "2022-05-01",
                    RefNo = "PA175",
                    TicketNo = "21",
                    Adults = 2,
                    Free = 1,
                    PutAt = "2023-09-07 09:55:16"
            }};
        }

        private static object[] Reservation_Must_Not_Be_Already_Updated() {
            return new object[] {
                new TestUpdateReservation {
                    StatusCode = 415,
                    ReservationId = Guid.Parse("08da2863-15d9-4338-81fa-637a52371163"),
                    CustomerId = 2,
                    DestinationId = 1,
                    Date = "2022-05-01",
                    PickupPointId = 215,
                    PortId = 2,
                    RefNo = "PA175",
                    TicketNo = "21",
                    Adults = 2,
                    PutAt = "2023-09-07 09:56:16"
                }
            };
        }

    }

}
