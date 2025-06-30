using System.Threading.Tasks;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceValidation {

        Task<int> IsValidAsync(Invoice x, InvoiceWriteDto invoice);

    }

}