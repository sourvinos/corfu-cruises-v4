using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Infrastructure.ReservationQueueServices {

    public interface IReservationProcessQueueRepository : IRepository<ReservationQueue> {

        Task<ReservationQueue> GetFirstNotImported();

    }

}