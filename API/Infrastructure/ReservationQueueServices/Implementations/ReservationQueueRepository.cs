using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using API.Infrastructure.Helpers;

namespace API.Infrastructure.ReservationQueueServices {

    public class ReservationQueueRepository : Repository<ReservationQueue>, IReservationQueueRepository {

        public ReservationQueueRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public async Task<ReservationQueue> GetFirstNotImported() {
            return await context.ReservationQueues
                .OrderBy(x => x.PostAt)
                .FirstOrDefaultAsync(x => x.IsImported == 0);
        }

        public async Task<bool> GetByDateAndTicketNoAsync(string date, string ticketNo) {
            var x = await context.Reservations
                .FirstOrDefaultAsync(x => x.Date == DateHelpers.StringToDate(date) && x.TicketNo == ticketNo);
            return x != null;
        }

        public async Task<ReservationQueue> GetByCode(string code) {
            return await context.ReservationQueues
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public void UpdateQueue(ReservationQueue queue) {
            context.ReservationQueues.Update(queue);
        }

    }

}