using System.Threading.Tasks;
using API.Infrastructure.EmailServices;

namespace API.Features.Sales.Invoices {

    public interface IEmailInvoiceSender {

        Task SendInvoiceToEmail(EmailQueue emailQueue, string email);

    }

}