using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.BankAccounts {

    public class BankAccountValidation : Repository<BankAccount>, IBankAccountValidation {

        public BankAccountValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(BankAccount z, BankAccountWriteDto bankAccount) {
            return true switch {
                var x when x == !IsValidShipOwner(bankAccount) => 449,
                var x when x == !IsValidBank(bankAccount) => 406,
                var x when x == IsAlreadyUpdated(z, bankAccount) => 415,
                _ => 200,
            };
        }

        private bool IsValidShipOwner(BankAccountWriteDto bankAccount) {
            return bankAccount.Id == 0
                ? context.ShipOwners
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == bankAccount.ShipOwnerId && x.IsActive) != null
                : context.ShipOwners
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == bankAccount.ShipOwnerId) != null;
        }

        private bool IsValidBank(BankAccountWriteDto bankAccount) {
            return bankAccount.Id == 0
                ? context.Banks
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == bankAccount.BankId && x.IsActive) != null
                : context.Banks
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == bankAccount.BankId) != null;
        }

        private static bool IsAlreadyUpdated(BankAccount z, BankAccountWriteDto bankAccount) {
            return z != null && z.PutAt != bankAccount.PutAt;
        }

    }

}