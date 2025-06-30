using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Invoices {

    public interface IInvoicePdfRepository {

        string BuildPdf(InvoicePdfVM invoice);
        string BuildMultiPagePdf(IEnumerable<InvoicePdfVM> invoices);
        FileStreamResult OpenPdf(string filename);

    }

}