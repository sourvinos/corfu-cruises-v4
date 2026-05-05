using System.Threading.Tasks;
using API.Features.Reservations.LinkTwist;
using API.Infrastructure.Interfaces;

namespace API.Infrastructure.ReservationQueueServices {

    public interface IReservationUpdateQueueRepository : IRepository<ReservationQueue> {

        Task<ReservationQueue> GetByCode(string code);
        Task<ReservationQueue> GetFirstNotCompleted();

    }

}