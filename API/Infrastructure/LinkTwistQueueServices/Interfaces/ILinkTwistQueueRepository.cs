using System.Threading.Tasks;
using API.Features.Reservations.LinkTwist;
using API.Infrastructure.Interfaces;

namespace API.Infrastructure.LinkTwistServices {

    public interface ILinkTwistQueueRepository : IRepository<LinkTwistQueue> {

        Task<LinkTwistQueue> GetFirstNotCompleted();

    }

}