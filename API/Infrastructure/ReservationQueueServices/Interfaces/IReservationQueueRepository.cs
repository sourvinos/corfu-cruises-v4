using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Infrastructure.ReservationQueueServices {

    public interface IReservationQueueRepository : IRepository<ReservationQueue> {

        Task<ReservationQueue> GetFirstNotImported();
        Task<ReservationQueue> GetByCode(string code);
        Task<bool> GetByDateAndTicketNoAsync(string date, string ticketNo);
        void UpdateQueue(ReservationQueue queue);

    }

}