using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Ships {

    public class ShipValidation : Repository<Ship>, IShipValidation {

        public ShipValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(Ship z, ShipWriteDto ship) {
            return true switch {
                var x when x == !IsValidShipOwner(ship) => 449,
                var x when x == IsAlreadyUpdated(z, ship) => 415,
                _ => 200,
            };
        }

        private bool IsValidShipOwner(ShipWriteDto ship) {
            return ship.Id == 0
                ? context.ShipOwners
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == ship.ShipOwnerId && x.IsActive) != null
                : context.ShipOwners
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == ship.ShipOwnerId) != null;
        }

        private static bool IsAlreadyUpdated(Ship z, ShipWriteDto ship) {
            return z != null && z.PutAt != ship.PutAt;
        }

    }

}