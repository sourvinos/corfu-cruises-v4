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

namespace API.Features.Reservations.Ports {

    public class PortRepository : Repository<Port>, IPortRepository {

        private readonly IMapper mapper;

        public PortRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PortListVM>> GetAsync() {
            var ports = await context.Ports
                .AsNoTracking()
                .OrderBy(x => x.StopOrder)
                .ToListAsync();
            return mapper.Map<IEnumerable<Port>, IEnumerable<PortListVM>>(ports);
        }

        public async Task<IEnumerable<PortBrowserVM>> GetForBrowserAsync() {
            var ports = await context.Ports
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Port>, IEnumerable<PortBrowserVM>>(ports);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var ports = await context.Ports
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Port>, IEnumerable<SimpleEntity>>(ports);
        }

        public async Task<Port> GetByIdAsync(int id) {
            return await context.Ports
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}