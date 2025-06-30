using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceReadRepository : IRepository<Invoice> {

        Task<IEnumerable<InvoiceListVM>> GetAsync();
        Task<IEnumerable<InvoiceListVM>> GetForPeriodAsync(InvoiceListCriteriaVM criteria);
        InvoicePdfVM GetFirstWithEmailPending();
        Task<Invoice> GetByIdAsync(string invoiceId, bool includeTables);
        Task<Invoice> GetByIdForPdfAsync(string invoiceId);
        Task<Invoice> GetByIdForPatchEmailSent(string invoiceId);
        Task<Invoice> GetByIdForXmlAsync(string invoiceId);
        Task<Invoice> GetByIdForJsonAsync(string invoiceId);
        Task<InvoiceAade> GetInvoiceAadeByIdAsync(string invoiceId);

    }

}