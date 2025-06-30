using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.BankAccounts {

    public interface IBankAccountRepository : IRepository<BankAccount> {

        Task<IEnumerable<BankAccountListVM>> GetAsync();
        Task<BankAccount> GetByIdAsync(int id, bool includeTables);

    }

}