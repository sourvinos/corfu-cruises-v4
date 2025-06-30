using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Infrastructure.EmailServices {

    public interface IEmailQueueRepository : IRepository<EmailQueue> {

        Task<EmailQueue> GetFirst();

    }

}