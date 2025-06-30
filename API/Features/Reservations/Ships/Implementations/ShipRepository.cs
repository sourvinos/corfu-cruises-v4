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

namespace API.Features.Reservations.Ships {

    public class ShipRepository : Repository<Ship>, IShipRepository {

        private readonly IMapper mapper;

        public ShipRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ShipListVM>> GetAsync() {
            var ships = await context.Ships
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Ship>, IEnumerable<ShipListVM>>(ships);
        }

        public async Task<IEnumerable<ShipBrowserVM>> GetForBrowserAsync() {
            var ships = await context.Ships
                .AsNoTracking()
                .Include(x => x.ShipOwner)
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Ship>, IEnumerable<ShipBrowserVM>>(ships);
        }

        public async Task<ShipBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.Ships
                .AsNoTracking()
                .Include(x => x.ShipOwner)
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<Ship, ShipBrowserVM>(record);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var ships = await context.Ships
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Ship>, IEnumerable<SimpleEntity>>(ships);
        }

        public async Task<Ship> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.Ships
                    .AsNoTracking()
                    .Include(x => x.ShipOwner)
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.Ships
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}