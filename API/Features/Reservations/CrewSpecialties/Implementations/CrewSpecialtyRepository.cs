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

namespace API.Features.Reservations.CrewSpecialties {

    public class CrewSpecialtyRepository : Repository<CrewSpecialty>, ICrewSpecialtyRepository {

        private readonly IMapper mapper;

        public CrewSpecialtyRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CrewSpecialtyBrowserVM>> GetBrowserAsync() {
            List<CrewSpecialty> activeCrewSpecialties = await context.CrewSpecialties
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<CrewSpecialty>, IEnumerable<CrewSpecialtyBrowserVM>>(activeCrewSpecialties);
        }

    }

}