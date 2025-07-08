using System.Threading.Tasks;

namespace API.Features.Sales.Ledgers {

    public interface ILedgerEmailSender {

        Task SendLedgerToEmail(EmailLedgerVM model);

    }

}