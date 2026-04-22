using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.LinkTwist {

    public class LinkTwistRepository : Repository<LinkTwistQueue>, ILinkTwistRepository {

        public LinkTwistRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
        }

        public async Task<LinkTwistQueue> GetByCode(string code) {
            var x = await context.LinkTwistQueues
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == code);
            return x;
        }

        public async Task<IEnumerable<LinkTwistQueue>> GetAsync() {
            var x = await context.LinkTwistQueues
                .AsNoTracking()
                .OrderBy(x => x.Code)
                .ToListAsync();
            return x;
        }

    }

}