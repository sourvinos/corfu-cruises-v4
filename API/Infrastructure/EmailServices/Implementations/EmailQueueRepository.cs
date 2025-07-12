using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.EmailServices {

    public class EmailQueueRepository : Repository<EmailQueue>, IEmailQueueRepository {

        public EmailQueueRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public async Task<EmailQueue> GetFirstNotCompleted() {
            return await context.EmailQueues
                .Where(x => !x.IsSent)
                .OrderBy(x => x.Priority).ThenBy(x => x.PostAt)
                .FirstOrDefaultAsync();
        }

        public async Task<EmailQueue> GetByIdAsync(int id) {
            return await context.EmailQueues
                .FirstOrDefaultAsync(x => x.Id == id);
        }

    }

}