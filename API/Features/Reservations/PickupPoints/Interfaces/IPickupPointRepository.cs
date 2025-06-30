using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.PickupPoints {

    public interface IPickupPointRepository : IRepository<PickupPoint> {

        Task<IEnumerable<PickupPointListVM>> GetAsync();
        Task<IEnumerable<PickupPointBrowserVM>> GetForBrowserAsync();
        Task<PickupPoint> GetByIdAsync(int id, bool includeTables);

    }

}