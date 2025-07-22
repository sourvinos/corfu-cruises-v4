using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Infrastructure.EmailServices {

    public interface IEmailQueueRepository : IRepository<EmailQueue> {

        Task<EmailQueue> GetFirstNotCompleted();
        Task<EmailQueue> GetByIdAsync(string entityId);
        EmailQueue CreateEmailQueue(EmailQueueDto emailQueue);

    }

}