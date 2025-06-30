using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Drivers{

    public interface IDriverRepository : IRepository<Driver> {

        Task<IEnumerable<DriverListVM>> GetAsync();
        Task<IEnumerable<DriverBrowserVM>> GetForBrowserAsync();
        Task<Driver> GetByIdAsync(int id);

    }

}