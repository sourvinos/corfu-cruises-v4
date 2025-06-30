using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.ShipOwners {

    public interface IShipOwnerRepository : IRepository<ShipOwner> {

        Task<IEnumerable<ShipOwnerListVM>> GetAsync();
        Task<IEnumerable<ShipOwnerBrowserVM>> GetForBrowserAsync();
        Task<ShipOwner> GetByIdAsync(int id);

    }

}