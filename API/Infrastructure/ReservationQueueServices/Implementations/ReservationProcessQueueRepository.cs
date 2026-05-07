using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;

namespace API.Infrastructure.ReservationQueueServices {

    public class ReservationProcessQueueRepository : Repository<ReservationQueue>, IReservationProcessQueueRepository {

        public ReservationProcessQueueRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public async Task<ReservationQueue> GetFirstNotImported() {
            return await context.ReservationQueues
                .OrderBy(x => x.PostAt)
                .FirstOrDefaultAsync(x => x.IsImported == 0);
        }

    }

}