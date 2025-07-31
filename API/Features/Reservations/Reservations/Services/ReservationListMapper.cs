using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;

namespace API.Features.Reservations.Reservations {

    public static class ReservationsListMapper {

        public static List<ReservationListVM> Map(IEnumerable<Reservation> reservations) {
            return reservations.Select(x => new ReservationListVM {
                ReservationId = x.ReservationId,
                Date = DateHelpers.DateToISOString(x.Date),
                RefNo = x.RefNo,
                TicketNo = x.TicketNo,
                Adults = x.Adults,
                Kids = x.Kids,
                Free = x.Free,
                TotalPax = x.TotalPax,
                Customer = new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description },
                CoachRoute = new ReservationListCoachRouteVM { Id = x.PickupPoint.CoachRoute.Id, Abbreviation = x.PickupPoint.CoachRoute.Abbreviation },
                Destination = new ReservationListDestinationVM { Id = x.Destination.Id, Abbreviation = x.Destination.Abbreviation },
                Driver = new ReservationListDriverVM { Id = x.Driver == null ? 0 : x.Driver.Id, Description = x.Driver == null ? "(EMPTY)" : x.Driver.Description, Phones = x.Driver == null ? "(EMPTY)" : x.Driver.Phones, },
                PickupPoint = new ReservationListPickupPointVM { Id = x.PickupPoint.Id, Description = x.PickupPoint.Description, Time = x.PickupPoint.Time },
                Port = new ReservationListPortVM { Id = x.Port.Id, Description = x.Port.Description, Abbreviation = x.Port.Abbreviation },
                PortAlternate = new ReservationListPortVM { Id = x.PortAlternate.Id, Description = x.PortAlternate.Description, Abbreviation = x.PortAlternate.Abbreviation },
                Ship = new SimpleEntity { Id = x.Ship == null ? 0 : x.Ship.Id, Description = x.Ship == null ? "(EMPTY)" : x.Ship.Abbreviation, },
                PassengerCount = x.Passengers.Count,
                PassengerDifference = x.TotalPax - x.Passengers.Count
            }).ToList();
        }

    }

}