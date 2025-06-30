using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Ledgers {

    public interface ILedgerEmailSender {

        Task SendLedgerToEmail(EmailLedgerVM model);
        FileStreamResult OpenPdf(string filename);

    }

}