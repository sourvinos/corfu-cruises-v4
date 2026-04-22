using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.LinkTwist {

    public interface ILinkTwistRepository : IRepository<LinkTwistQueue> {

        Task<LinkTwistQueue> GetByCode(string code);
        Task<IEnumerable<LinkTwistQueue>> GetAsync();

    }

}