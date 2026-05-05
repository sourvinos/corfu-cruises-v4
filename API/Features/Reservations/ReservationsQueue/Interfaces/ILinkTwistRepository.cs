using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.LinkTwist {

    public interface ILinkTwistRepository : IRepository<ReservationQueue> {

        Task<ReservationQueue> GetByCode(string code);
        Task<IEnumerable<ReservationQueue>> GetAsync();

    }

}