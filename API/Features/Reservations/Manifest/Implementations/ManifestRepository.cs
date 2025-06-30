using System.Linq;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;
using API.Features.Reservations.ShipCrews;

namespace API.Features.Reservations.Manifest {

    public class ManifestRepository : Repository<Reservation>, IManifestRepository {

        private readonly IMapper mapper;

        public ManifestRepository(AppDbContext appDbContext, IMapper mapper, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ManifestPassengerVM>> GetPassengersAsync(string date, int destinationId, int portId, int shipId) {
            var passengers = await context.Passengers
                .AsNoTracking()
                .Include(x => x.Nationality)
                .Include(x => x.Gender)
                .Include(x => x.Reservation).ThenInclude(x => x.PortAlternate)
                .OrderBy(x => x.Lastname).ThenBy(x => x.Firstname).ThenBy(x => x.Birthdate)
                .Where(x => x.Reservation.Date.ToString() == date
                    && x.Reservation.DestinationId == destinationId
                    && x.Reservation.ShipId == shipId
                    && x.Reservation.PortAlternateId == portId
                    && x.IsBoarded)
                .ToListAsync();
            return mapper.Map<IEnumerable<Passenger>, IEnumerable<ManifestPassengerVM>>(passengers);
        }

        public async Task<IEnumerable<ManifestCrewVM>> GetCrewAsync(int shipId) {
            var crew = await context.ShipCrews
                .AsNoTracking()
                .Include(x => x.Nationality)
                .Include(x => x.Gender)
                .Include(x => x.Specialty)
                .Where(x => x.Ship.Id == shipId && x.IsActive)
                .ToListAsync();
            return mapper.Map<IEnumerable<ShipCrew>, IEnumerable<ManifestCrewVM>>(crew);
        }

    }

}