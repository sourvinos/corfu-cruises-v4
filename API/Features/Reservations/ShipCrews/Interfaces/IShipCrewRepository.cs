using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.ShipCrews {

    public interface IShipCrewRepository : IRepository<ShipCrew> {

        Task<IEnumerable<ShipCrewListVM>> GetAsync();
        Task<ShipCrew> GetByIdAsync(int id, bool includeTables);

    }

}