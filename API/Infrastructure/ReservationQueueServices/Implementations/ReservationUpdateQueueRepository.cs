using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.ReservationQueueServices {

    public class ReservationUpdateQueueRepository : Repository<ReservationQueue>, IReservationUpdateQueueRepository {

        public ReservationUpdateQueueRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public async Task<ReservationQueue> GetByCode(string code) {
            return await context.ReservationQueues
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public void UpdateQueue(ReservationQueue queue) {
            context.ReservationQueues.Update(queue);
        }
    }

}