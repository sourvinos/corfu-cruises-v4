using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.CoachRoutes {

    public interface ICoachRouteRepository : IRepository<CoachRoute> {

        Task<IEnumerable<CoachRouteListVM>> GetAsync();
        Task<IEnumerable<CoachRouteBrowserVM>> GetForBrowserAsync();
        Task<CoachRoute> GetByIdAsync(int id);

    }

}