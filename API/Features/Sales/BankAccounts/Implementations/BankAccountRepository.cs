using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.BankAccounts {

    public class BankAccountRepository : Repository<BankAccount>, IBankAccountRepository {

        private readonly IMapper mapper;

        public BankAccountRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BankAccountListVM>> GetAsync() {
            var bankAccounts = await context.BankAccounts
                .AsNoTracking()
                .Include(x => x.Bank)
                .Include(x => x.ShipOwner)
                .OrderBy(x => x.ShipOwner.Description).ThenBy(x => x.Bank.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<BankAccount>, IEnumerable<BankAccountListVM>>(bankAccounts);
        }

        public async Task<BankAccount> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.BankAccounts
                    .AsNoTracking()
                    .Include(x => x.Bank)
                    .Include(x => x.ShipOwner)
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.BankAccounts
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}