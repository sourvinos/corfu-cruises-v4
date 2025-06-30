using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.ShipCrews {

    public class ShipCrewRepository : Repository<ShipCrew>, IShipCrewRepository {

        private readonly IMapper mapper;

        public ShipCrewRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ShipCrewListVM>> GetAsync() {
            var shipCrews = await context.ShipCrews
                .AsNoTracking()
                .Include(x => x.Ship)
                .Include(x => x.Gender)
                .Include(x => x.Nationality)
                .Include(x => x.Specialty)
                .OrderBy(x => x.Ship.Description).ThenBy(x => x.Lastname).ThenBy(x => x.Firstname).ThenBy(x => x.Birthdate)
                .ToListAsync();
            return mapper.Map<IEnumerable<ShipCrew>, IEnumerable<ShipCrewListVM>>(shipCrews);
        }

        public async Task<ShipCrew> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.ShipCrews
                    .AsNoTracking()
                    .Include(x => x.Ship)
                    .Include(x => x.Gender)
                    .Include(x => x.Nationality)
                    .Include(x => x.Specialty)
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.ShipCrews
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}