using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.Parameters {

    public class SaleParametersRepository : Repository<SaleParameter>, ISaleParametersRepository {

        public SaleParametersRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> boosettings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, boosettings, userManager) { }

        public async Task<SaleParameter> GetAsync() {
            return await context.SaleParameters
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

    }

}