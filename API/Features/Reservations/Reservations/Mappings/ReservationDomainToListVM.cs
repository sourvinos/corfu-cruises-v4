using System.Collections.Generic;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;

namespace API.Features.Reservations.Reservations {

    public static class ReservationMappingReadDomainToListVM {

        public static IEnumerable<ReservationListVM> ReservationDomainToListVM(IEnumerable<Reservation> reservations) {
            var list = new List<ReservationListVM>();
            foreach (var x in reservations) {
                var i = new ReservationListVM {
                    ReservationId = x.ReservationId,
                    Date = DateHelpers.DateToISOString(x.Date),
                    RefNo = x.RefNo,
                    TicketNo = x.TicketNo,
                    Adults = x.Adults,
                    Kids = x.Kids,
                    Free = x.Free,
                    TotalPax = x.TotalPax,
                    Customer = new SimpleEntity {
                        Id = x.Customer.Id,
                        Description = x.Customer.Description
                    },
                    CoachRoute = new ReservationListCoachRouteVM {
                        Id = x.PickupPoint.CoachRoute.Id,
                        Abbreviation = x.PickupPoint.CoachRoute.Abbreviation
                    },
                    Destination = new ReservationListDestinationVM {
                        Id = x.Destination.Id,
                        Abbreviation = x.Destination.Abbreviation,
                        Description = x.Destination.Description
                    },
                    Driver = new ReservationListDriverVM {
                        Id = x.Driver != null ? x.Driver.Id : 0,
                        Description = x.Driver != null ? x.Driver.Description : "(EMPTY)",
                        Phones = x.Driver != null ? x.Driver.Phones : ""
                    },
                    PickupPoint = new ReservationListPickupPointVM {
                        Id = x.PickupPoint.Id,
                        Description = x.PickupPoint.Description,
                        Time = x.PickupPoint.Time
                    },
                    Port = new ReservationListPortVM {
                        Id = x.Port.Id,
                        Abbreviation = x.Port.Abbreviation,
                        Description = x.Port.Description
                    },
                    PortAlternate = new ReservationListPortVM {
                        Id = x.PortAlternate.Id,
                        Abbreviation = x.PortAlternate.Abbreviation,
                        Description = x.PortAlternate.Description
                    },
                    Ship = new ReservationListShipVM {
                        Id = x.Ship != null ? x.Ship.Id : 0,
                        Abbreviation = x.Ship != null ? x.Ship.Abbreviation : "(EMPTY)",
                        Description = x.Ship != null ? x.Ship.Description : "(EMPTY)"
                    },
                    PassengerCount = x.Passengers.Count,
                    PassengerDifference = x.TotalPax - x.Passengers.Count
                };
                list.Add(i);
            }
            return list;
        }

    }

}