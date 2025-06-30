using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.PickupPoints {

    public class PickupPointValidation : Repository<PickupPoint>, IPickupPointValidation {

        public PickupPointValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(PickupPoint z, PickupPointWriteDto pickupPoint) {
            return true switch {
                var x when x == !IsValidCoachRoute(pickupPoint) => 408,
                var x when x == !IsValidPort(pickupPoint) => 411,
                var x when x == IsAlreadyUpdated(z, pickupPoint) => 415,
                _ => 200,
            };
        }

        private bool IsValidCoachRoute(PickupPointWriteDto pickupPoint) {
            return pickupPoint.Id == 0
                ? context.CoachRoutes
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == pickupPoint.CoachRouteId && x.IsActive) != null
                : context.CoachRoutes
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == pickupPoint.CoachRouteId) != null;
        }

        private bool IsValidPort(PickupPointWriteDto pickupPoint) {
            return pickupPoint.Id == 0
                ? context.Ports
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == pickupPoint.PortId && x.IsActive) != null
                : context.Ports
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == pickupPoint.PortId) != null;
        }

        private static bool IsAlreadyUpdated(PickupPoint z, PickupPointWriteDto pickupPoint) {
            return z != null && z.PutAt != pickupPoint.PutAt;
        }

    }

}