using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Ledgers {

    public interface ILedgerPdfBuilder {

        Task<string> CreatePdfLedger(LedgerCriteria criteria, int ShipOwnerId);
        FileStreamResult OpenPdf(string filename);

    }

}