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

namespace API.Features.Reservations.CoachRoutes {

    public class CoachRouteRepository : Repository<CoachRoute>, ICoachRouteRepository {

        private readonly IMapper mapper;

        public CoachRouteRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(context, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CoachRouteListVM>> GetAsync() {
            var coachRoutes = await context.CoachRoutes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<CoachRoute>, IEnumerable<CoachRouteListVM>>(coachRoutes);
        }

        public async Task<IEnumerable<CoachRouteBrowserVM>> GetForBrowserAsync() {
            var coachRoutes = await context.CoachRoutes
                .AsNoTracking()
                .OrderBy(x => x.Abbreviation)
                .ToListAsync();
            return mapper.Map<IEnumerable<CoachRoute>, IEnumerable<CoachRouteBrowserVM>>(coachRoutes);
        }

        public async Task<CoachRoute> GetByIdAsync(int id) {
            return await context.CoachRoutes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}