using System;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceUpdateRepository : IRepository<Invoice> {

        Invoice Update(Guid id, Invoice invoice);
        InvoiceAade UpdateInvoiceAade(InvoiceAade invoiceAade);
        InvoiceAade UpdateInvoiceOxygen(InvoiceAade invoiceAade);
        void UpdateIsEmailSent(Invoice invoice, string invoiceId);
        void UpdateIsEmailPending(Invoice invoice, string invoiceId);
        void UpdateIsCancelled(Invoice invoice, string invoiceId);
        Task<int> IncreaseInvoiceNoAsync(InvoiceCreateDto invoice);
        Task<int> AttachShipOwnerIdToInvoiceAsync(InvoiceCreateDto invoice);

    }

}