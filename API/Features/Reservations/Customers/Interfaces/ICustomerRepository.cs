using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Customers {

    public interface ICustomerRepository : IRepository<Customer> {

        Task<IEnumerable<CustomerListVM>> GetAsync();
        Task<IEnumerable<CustomerBrowserVM>> GetForBrowserAsync();
        Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync();
        Task<CustomerBrowserVM> GetByIdForBrowserAsync(int id);
        CustomerValidVM GetCustomerData(Customer x);
        Task<Customer> GetByIdAsync(int id, bool includeTables);
        Task<IList<CustomerListVM>> GetForBalanceSheetAsync();

    }

}