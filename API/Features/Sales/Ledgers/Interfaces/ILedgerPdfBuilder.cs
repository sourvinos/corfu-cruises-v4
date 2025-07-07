using System.Threading.Tasks;
using API.Infrastructure.EmailServices;

namespace API.Features.Sales.Ledgers {

    public interface ILedgerPdfBuilder {

        Task<EmailLedgerVM> CreatePdfLedger(EmailQueue emailQueue);

    }

}