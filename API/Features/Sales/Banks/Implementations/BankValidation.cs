using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.Banks {

    public class BankValidation : Repository<Bank>, IBankValidation {

        public BankValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(Bank z, BankWriteDto bank) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, bank) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Bank z, BankWriteDto bank) {
            return z != null && z.PutAt != bank.PutAt;
        }

    }

}