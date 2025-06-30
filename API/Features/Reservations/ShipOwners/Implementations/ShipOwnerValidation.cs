using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Features.Reservations.ShipOwners {

    public class ShipOwnerValidation : Repository<ShipOwner>, IShipOwnerValidation {

        public ShipOwnerValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public async Task<int> IsValidAsync(ShipOwner z, ShipOwnerWriteDto shipOwner) {
            return true switch {
                var x when x == !await IsValidNationality(shipOwner) => 456,
                var x when x == !await IsValidTaxOffice(shipOwner) => 458,
                var x when x == IsAlreadyUpdated(z, shipOwner) => 415,
                _ => 200,
            };
        }

        private async Task<bool> IsValidNationality(ShipOwnerWriteDto shipOwner) {
            if (shipOwner.Id == 0) {
                return await context.Nationalities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == shipOwner.NationalityId && x.IsActive) != null;
            }
            return await context.Nationalities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == shipOwner.NationalityId) != null;
        }

        private async Task<bool> IsValidTaxOffice(ShipOwnerWriteDto shipOwner) {
            if (shipOwner.Id == 0) {
                return await context.TaxOffices
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == shipOwner.TaxOfficeId && x.IsActive) != null;
            }
            return await context.TaxOffices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == shipOwner.TaxOfficeId) != null;
        }

        private static bool IsAlreadyUpdated(ShipOwner z, ShipOwnerWriteDto ship) {
            return z != null && z.PutAt != ship.PutAt;
        }

    }

}