using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Banks {

    public interface IBankRepository : IRepository<Bank> {

        Task<IEnumerable<BankListVM>> GetAsync();
        Task<IEnumerable<BankBrowserVM>> GetForBrowserAsync();
        Task<Bank> GetByIdAsync(string id);
 
    }

}