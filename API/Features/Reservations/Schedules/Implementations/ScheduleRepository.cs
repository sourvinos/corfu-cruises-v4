using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Schedules {

    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository {

        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<UserExtended> userManager;

        public ScheduleRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(context, httpContextAccessor, settings, userManager) {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<ScheduleListVM>> GetAsync() {
            var schedules = await context.Schedules
                .AsNoTracking()
                .Include(x => x.Destination)
                .Include(x => x.Port)
                .OrderBy(x => x.Date).ThenBy(x => x.Destination.Description).ThenBy(x => x.Port.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Schedule>, IEnumerable<ScheduleListVM>>(schedules);
        }

        public async Task<Schedule> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.Schedules
                    .AsNoTracking()
                    .Include(x => x.Port)
                    .Include(p => p.Destination)
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.Schedules
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

        public List<ScheduleWriteDto> AttachMetadataToPostDto(List<ScheduleWriteDto> schedules) {
            schedules.ForEach(x => x.PostAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime()));
            schedules.ForEach(x => x.PostUser = Identity.GetConnectedUserDetails(userManager, Identity.GetConnectedUserId(httpContextAccessor)).UserName);
            schedules.ForEach(x => x.PutAt = x.PostAt);
            schedules.ForEach(x => x.PutUser = x.PostUser);
            return schedules;
        }

    }

}