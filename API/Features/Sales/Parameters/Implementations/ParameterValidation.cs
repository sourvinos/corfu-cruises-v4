using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.Parameters {

    public class SaleParameterValidation : Repository<SaleParameter>, ISaleParameterValidation {

        public SaleParameterValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(SaleParameter z, ParameterWriteDto parameter) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, parameter) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(SaleParameter z, ParameterWriteDto parameter) {
            return z != null && z.PutAt != parameter.PutAt;
        }

    }

}