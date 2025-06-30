using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.CrewSpecialties {

    public interface ICrewSpecialtyRepository : IRepository<CrewSpecialty> {

        Task<IEnumerable<CrewSpecialtyBrowserVM>> GetBrowserAsync();

    }

}