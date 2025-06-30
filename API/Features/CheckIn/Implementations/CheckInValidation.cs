using System;
using System.Linq;
using API.Features.Reservations.PickupPoints;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace API.Features.CheckIn {

    public class CheckInValidation : ICheckInValidation {

        protected readonly AppDbContext context;

        public CheckInValidation(AppDbContext context) {
            this.context = context;
        }

        public int IsValidOnRead(Reservation z) {
            return true switch {
                var x when x == CheckInNotAllowedAfterDeparture(z) => 403,
                _ => 200,
            };
        }

        public int IsValidOnUpdate(Reservation z, ReservationWriteDto reservation) {
            return true switch {
                var x when x == CheckInNotAllowedAfterDeparture(z) => 403,
                var x when x == IsAlreadyUpdated(z, reservation) => 415,
                _ => 200,
            };
        }
 
        public int GetPortIdFromPickupPointId(Reservation reservation) {
            PickupPoint pickupPoint = context.PickupPoints
                .AsNoTracking()
                .SingleOrDefault(x => x.Id == reservation.PickupPointId);
            return pickupPoint != null
                ? (reservation.PortId != 0 && pickupPoint.PortId != reservation.PortId) ? reservation.PortId : pickupPoint.PortId
                : 0;
        }

        private bool CheckInNotAllowedAfterDeparture(Reservation reservation) {
            return IsAfterDeparture(reservation);
        }

        private bool IsAfterDeparture(Reservation reservation) {
            var timeNow = DateHelpers.GetLocalDateTime();
            var departureTime = GetScheduleDepartureTime(reservation);
            return DateTime.Compare(timeNow, departureTime) > 0;
        }

        private DateTime GetScheduleDepartureTime(Reservation reservation) {
            var portId = GetPortIdFromPickupPointId(reservation).ToString();
            var schedule = context.Schedules
                .AsNoTracking()
                .Where(x => x.Date == reservation.Date && x.DestinationId == reservation.DestinationId && x.PortId.ToString() == portId)
                .SingleOrDefault();
            var departureTime = schedule.Date.ToString("yyyy-MM-dd") + " " + schedule.Time;
            var departureTimeAsDate = DateTime.Parse(departureTime);
            return departureTimeAsDate;
        }

        private static bool IsAlreadyUpdated(Reservation z, ReservationWriteDto reservation) {
            return z != null && z.PutAt != reservation.PutAt;
        }

    }

}