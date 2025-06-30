using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Ships {

    public interface IShipRepository : IRepository<Ship> {

        Task<IEnumerable<ShipListVM>> GetAsync();
        Task<IEnumerable<ShipBrowserVM>> GetForBrowserAsync();
        Task<ShipBrowserVM> GetByIdForBrowserAsync(int id);
        Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync();
        Task<Ship> GetByIdAsync(int id, bool includeTables);

    }

}