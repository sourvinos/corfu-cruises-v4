using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Parameters {

    public class ParametersRepository : Repository<ReservationParameter>, IReservationParametersRepository {

        public ParametersRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> boosettings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, boosettings, userManager) { }

        public async Task<ReservationParameter> GetAsync() {
            return await context.ReservationParameters
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

    }

}