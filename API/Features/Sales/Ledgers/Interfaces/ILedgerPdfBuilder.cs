using System.Threading.Tasks;
using API.Infrastructure.EmailServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Ledgers {

    public interface ILedgerPdfBuilder {

        Task<string> CreatePdfLedger(LedgerCriteria criteria);
        Task<EmailLedgerVM> CreatePdfLedger(EmailQueue emailQueue);
        FileStreamResult OpenPdf(string filename);

    }

}