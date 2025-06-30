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

namespace API.Features.Reservations.Drivers{

    public class DriverRepository : Repository<Driver>, IDriverRepository {

        private readonly IMapper mapper;

        public DriverRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<DriverListVM>> GetAsync() {
            List<Driver> drivers = await context.Drivers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Driver>, IEnumerable<DriverListVM>>(drivers);
        }

        public async Task<IEnumerable<DriverBrowserVM>> GetForBrowserAsync() {
            List<Driver> drivers = await context.Drivers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Driver>, IEnumerable<DriverBrowserVM>>(drivers);
        }

        public async Task<Driver> GetByIdAsync(int id) {
            return await context.Drivers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}