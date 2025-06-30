using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Helpers;
using API.Infrastructure.Extensions;

namespace API.Features.Reservations.Reservations {

    public class ReservationCloneRepository : Repository<Reservation>, IReservationCloneRepository {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly TestingEnvironment testingEnvironment;
        private readonly UserManager<UserExtended> userManager;

        public ReservationCloneRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContextAccessor, testingEnvironment, userManager) {
            this.httpContextAccessor = httpContextAccessor;
            this.testingEnvironment = testingEnvironment.Value;
            this.userManager = userManager;
        }

        public int Clone(IEnumerable<CloneReservationVM> reservations) {
            foreach (CloneReservationVM reservation in reservations) {
                var x = new Reservation() {
                    Date = DateHelpers.StringToDate(reservation.Date),
                    CustomerId = reservation.CustomerId,
                    DestinationId = reservation.DestinationId,
                    PickupPointId = reservation.PickupPointId,
                    PortId = reservation.PortId,
                    PortAlternateId = reservation.PortId,
                    TicketNo = "auto-generated",
                    Adults = reservation.Adults,
                    Kids = reservation.Kids,
                    Free = reservation.Free,
                    Remarks = reservation.Remarks,
                    PostAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime()),
                    PostUser = Identity.GetConnectedUserDetails(userManager, Identity.GetConnectedUserId(httpContextAccessor)).UserName
                };
                using var transaction = context.Database.BeginTransaction();
                context.Reservations.Add(x);
                context.SaveChanges();
                if (testingEnvironment.IsTesting) {
                    transaction.Dispose();
                } else {
                    transaction.Commit();
                }
            }
            return reservations.Count();
        }

    }

}