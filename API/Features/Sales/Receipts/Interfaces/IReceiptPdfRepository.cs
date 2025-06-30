using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Receipts {

    public interface IReceiptPdfRepository {

        string BuildPdf(ReceiptPdfVM receipt);
        FileStreamResult OpenPdf(string filename);

    }

}