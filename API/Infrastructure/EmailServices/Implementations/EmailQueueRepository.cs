using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using API.Infrastructure.Helpers;

namespace API.Infrastructure.EmailServices {

    public class EmailQueueRepository : Repository<EmailQueue>, IEmailQueueRepository {

        public EmailQueueRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public async Task<EmailQueue> GetFirstNotCompleted() {
            return await context.EmailQueues
                .Where(x => !x.IsSent)
                .OrderBy(x => x.Priority).ThenBy(x => x.PostAt)
                .FirstOrDefaultAsync();
        }

        public async Task<EmailQueue> GetByIdAsync(string entityId) {
            return await context.EmailQueues
                .FirstOrDefaultAsync(x => x.EntityId.ToString() == entityId);
        }

        public EmailQueue CreateEmailQueue(EmailQueueDto emailQueue) {
            return new EmailQueue {
                EntityId = IsNotGuid(emailQueue.EntityId) ? Guid.NewGuid() : emailQueue.EntityId,
                Initiator = emailQueue.Initiator,
                FromDate = emailQueue.FromDate != null ? emailQueue.FromDate : null,
                ToDate = emailQueue.ToDate != null ? emailQueue.ToDate : null,
                CustomerId = emailQueue.CustomerId,
                Priority = emailQueue.Priority,
                IsSent = false,
                PostAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime())
            };
        }

        private static bool IsNotGuid(Guid x) {
            return x.ToString().Count(x => x == '0') == 32;
        }

    }

}