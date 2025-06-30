using System.Threading.Tasks;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceEmailSender {

        Task SendInvoicesToEmail(EmailInvoicesVM model);

    }

}