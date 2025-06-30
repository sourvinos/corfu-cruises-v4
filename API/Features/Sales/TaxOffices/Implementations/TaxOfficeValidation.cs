using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.TaxOffices {

    public class TaxOfficeValidation : Repository<TaxOffice>, ITaxOfficeValidation {

        public TaxOfficeValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(TaxOffice z, TaxOfficeWriteDto taxOffice) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, taxOffice) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(TaxOffice z, TaxOfficeWriteDto taxOffice) {
            return z != null && z.PutAt != taxOffice.PutAt;
        }

    }

}