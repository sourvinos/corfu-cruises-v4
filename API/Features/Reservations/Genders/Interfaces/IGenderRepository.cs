using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Genders {

    public interface IGenderRepository : IRepository<Gender> {

        Task<IEnumerable<GenderBrowserVM>> GetForBrowserAsync();

    }

}