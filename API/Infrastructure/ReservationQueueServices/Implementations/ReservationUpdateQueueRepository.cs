using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Features.Reservations.LinkTwist;
using System.Linq;

namespace API.Infrastructure.ReservationQueueServices {

    public class ReservationUpdateQueueRepository : Repository<ReservationQueue>, IReservationUpdateQueueRepository {

        public ReservationUpdateQueueRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public async Task<ReservationQueue> GetByCode(string code) {
            return await context.LinkTwistQueues
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<ReservationQueue> GetFirstNotCompleted() {
            return await context.LinkTwistQueues
                .OrderBy(x => x.PostAt)
                .FirstOrDefaultAsync(x => !x.IsImported);
        }

    }

}