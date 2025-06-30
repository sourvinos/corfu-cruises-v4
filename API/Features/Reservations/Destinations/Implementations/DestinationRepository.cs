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

namespace API.Features.Reservations.Destinations {

    public class DestinationRepository : Repository<Destination>, IDestinationRepository {

        private readonly IMapper mapper;

        public DestinationRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<DestinationListVM>> GetAsync() {
            var destinations = await context.Destinations
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationListVM>>(destinations);
        }

        public async Task<IEnumerable<DestinationBrowserVM>> GetForBrowserAsync() {
            var destinations = await context.Destinations
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationBrowserVM>>(destinations);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var destinations = await context.Destinations
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Destination>, IEnumerable<SimpleEntity>>(destinations);
        }

        public async Task<Destination> GetByIdAsync(int id) {
            return await context.Destinations
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}