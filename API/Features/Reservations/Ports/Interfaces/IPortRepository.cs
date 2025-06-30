using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Ports {

    public interface IPortRepository : IRepository<Port> {

        Task<IEnumerable<PortListVM>> GetAsync();
        Task<IEnumerable<PortBrowserVM>> GetForBrowserAsync();
        Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync();
        Task<Port> GetByIdAsync(int id);

    }

}