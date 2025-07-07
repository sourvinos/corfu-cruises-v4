using System.Threading.Tasks;
using API.Infrastructure.EmailServices;

namespace API.Features.Sales.Receipts {

    public interface IReceiptEmailSender {

        Task SendReceiptToEmail(EmailQueue emailQueue, string email);

    }

}