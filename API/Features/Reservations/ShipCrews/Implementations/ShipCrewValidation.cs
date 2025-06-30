using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.ShipCrews {

    public class ShipCrewValidation : Repository<ShipCrew>, IShipCrewValidation {

        public ShipCrewValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(ShipCrew z, ShipCrewWriteDto shipCrew) {
            return true switch {
                var x when x == !IsValidGender(shipCrew) => 457,
                var x when x == !IsValidNationality(shipCrew) => 456,
                var x when x == !IsValidShip(shipCrew) => 454,
                var x when x == !IsValidSpecialty(shipCrew) => 464,
                var x when x == IsAlreadyUpdated(z, shipCrew) => 415,
                _ => 200,
            };
        }

        private bool IsValidGender(ShipCrewWriteDto shipCrew) {
            return shipCrew.Id == 0
                ? context.Genders
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == shipCrew.GenderId && x.IsActive) != null
                : context.Genders
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == shipCrew.GenderId) != null;
        }

        private bool IsValidNationality(ShipCrewWriteDto shipCrew) {
            return shipCrew.Id == 0
                ? context.Nationalities
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == shipCrew.NationalityId && x.IsActive) != null
                : context.Nationalities
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == shipCrew.NationalityId) != null;
        }

        private bool IsValidShip(ShipCrewWriteDto shipCrew) {
            return shipCrew.Id == 0
                ? context.Ships
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == shipCrew.ShipId && x.IsActive) != null
                : context.Ships
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == shipCrew.ShipId) != null;
        }

        private bool IsValidSpecialty(ShipCrewWriteDto shipCrew) {
            return shipCrew.Id == 0
                ? context.CrewSpecialties
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == shipCrew.SpecialtyId && x.IsActive) != null
                : context.CrewSpecialties
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == shipCrew.SpecialtyId) != null;
        }

        private static bool IsAlreadyUpdated(ShipCrew z, ShipCrewWriteDto shipCrew) {
            return z != null && z.PutAt != shipCrew.PutAt;
        }

    }

}