using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Infrastructure.ReservationQueueServices {

    public interface IReservationUpdateQueueRepository : IRepository<ReservationQueue> {

        Task<ReservationQueue> GetByCode(string code);
        void UpdateQueue(ReservationQueue queue);

    }

}