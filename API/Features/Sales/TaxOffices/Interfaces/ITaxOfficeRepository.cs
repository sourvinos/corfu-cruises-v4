using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.TaxOffices {

    public interface ITaxOfficeRepository : IRepository<TaxOffice> {

        Task<IEnumerable<TaxOfficeListVM>> GetAsync();
        Task<IEnumerable<TaxOfficeBrowserVM>> GetForBrowserAsync();
        Task<TaxOffice> GetByIdAsync(int id);
 
    }

}