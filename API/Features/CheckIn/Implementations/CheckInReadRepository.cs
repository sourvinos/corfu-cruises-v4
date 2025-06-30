using System;
using System.Linq;
using System.Threading.Tasks;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Classes;
using Microsoft.EntityFrameworkCore;

namespace API.Features.CheckIn {

    public class CheckInReadRepository : ICheckInReadRepository {

        protected readonly AppDbContext context;

        public CheckInReadRepository(AppDbContext context) {
            this.context = context;
        }

        public async Task<Reservation> GetByRefNo(string refNo) {
            var reservation = context.Reservations
               .AsNoTracking()
               .Include(x => x.Customer)
               .Include(x => x.Destination)
               .Include(x => x.PickupPoint).ThenInclude(x => x.Port)
               .Include(x => x.Passengers).ThenInclude(x => x.Nationality)
               .Include(x => x.Passengers).ThenInclude(x => x.Occupant)
               .Include(x => x.Passengers).ThenInclude(x => x.Gender)
               .Where(x => x.RefNo.ToLower() == refNo.ToLower())
               .FirstOrDefaultAsync();
            return await reservation;
        }

        public async Task<Reservation> GetByDate(string date, int destinationId, string lastname, string firstname) {
            var reservation = context.Reservations
               .AsNoTracking()
               .Include(x => x.Customer)
               .Include(x => x.Destination)
               .Include(x => x.PickupPoint)
               .Include(x => x.Passengers).ThenInclude(x => x.Nationality)
               .Include(x => x.Passengers).ThenInclude(x => x.Occupant)
               .Include(x => x.Passengers).ThenInclude(x => x.Gender)
               .Where(x => x.Date == Convert.ToDateTime(date)
                    && x.DestinationId == destinationId
                    && x.Passengers.Any(x => x.Lastname.Trim().ToLower() == lastname.Trim().ToLower())
                    && x.Passengers.Any(x => x.Firstname.Trim().ToLower() == firstname.Trim().ToLower()))
                .FirstOrDefaultAsync();
            return await reservation;
        }

        public async Task<Reservation> GetByIdAsync(string reservationId, bool includeTables) {
            return includeTables
                ? await context.Reservations
                    .AsNoTracking()
                    .Include(x => x.Customer)
                    .Include(x => x.PickupPoint)
                    .Include(x => x.Destination)
                    .Include(x => x.Passengers).ThenInclude(x => x.Nationality)
                    .Include(x => x.Passengers).ThenInclude(x => x.Occupant)
                    .Include(x => x.Passengers).ThenInclude(x => x.Gender)
                    .Where(x => x.ReservationId.ToString() == reservationId)
                    .SingleOrDefaultAsync()
                : await context.Reservations
                    .AsNoTracking()
                    .Include(x => x.Passengers)
                    .Where(x => x.ReservationId.ToString() == reservationId)
                    .SingleOrDefaultAsync();
        }

        public async Task<Reservation> GetByIdForPatchEmailSent(string reservationId) {
            return await context.Reservations
                .AsNoTracking()
                .Where(x => x.ReservationId.ToString() == reservationId)
                .SingleOrDefaultAsync();
        }

        public Reservation GetFirstWithEmailPending() {
            var x = context.Reservations
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Destination)
                .Include(x => x.Passengers).ThenInclude(x => x.Gender)
                .Include(x => x.Passengers).ThenInclude(x => x.Nationality)
                .Include(x => x.Passengers).ThenInclude(x => x.Occupant)
                .Include(x => x.PickupPoint)
                .Where(x => x.IsEmailPending)
                .FirstOrDefault();
            return x;
        }

        public Task SendReservationToEmail(BoardingPassReservationVM reservation) {
            throw new NotImplementedException();
        }

    }

}