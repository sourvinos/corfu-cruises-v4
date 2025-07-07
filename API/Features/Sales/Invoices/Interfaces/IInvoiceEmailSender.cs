using System.Threading.Tasks;
using API.Infrastructure.EmailServices;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceEmailSender {

        Task SendInvoiceToEmail(EmailQueue emailQueue, string email);

    }

}