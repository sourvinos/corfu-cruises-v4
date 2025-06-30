using System;
using System.Threading;
using System.Threading.Tasks;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Classes;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API.Features.CheckIn {

    public class CheckInEmailScheduleService : BackgroundService {

        #region variables

        private readonly IReservationReadRepository reservationReadRepo;
        private readonly ICheckInReadRepository checkInReadRepo;
        private readonly IServiceProvider serviceProvider;
        private readonly ICheckInSendToEmail checkInSendToEmail;
        private readonly IMapper mapper;

        #endregion

        public CheckInEmailScheduleService(ICheckInSendToEmail checkInSendToEmail, ICheckInReadRepository checkInReadRepo, IReservationReadRepository reservationReadRepo, IServiceProvider serviceProvider, IMapper mapper) {
            this.reservationReadRepo = reservationReadRepo;
            this.checkInReadRepo = checkInReadRepo;
            this.checkInSendToEmail = checkInSendToEmail;
            this.serviceProvider = serviceProvider;
            this.mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(150), stoppingToken);
                var x = checkInReadRepo.GetFirstWithEmailPending();
                if (x != null) {
                    var z = mapper.Map<Reservation, CheckInBoardingPassReservationVM>(x);
                    await checkInSendToEmail.SendReservationToEmail(z);
                    await PatchReservationEmailFields(x);
                }
            }
        }

        private async Task PatchReservationEmailFields(Reservation x) {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var reservation = await reservationReadRepo.GetByIdForPatchEmailSent(x.ReservationId.ToString());
            reservation.IsEmailPending = false;
            reservation.IsEmailSent = true;
            dbContext.Reservations.Attach(reservation);
            dbContext.Entry(reservation).Property(x => x.IsEmailPending).IsModified = true;
            dbContext.Entry(reservation).Property(x => x.IsEmailSent).IsModified = true;
            dbContext.SaveChanges();
        }

    }

}