using System.Threading.Tasks;

namespace API.Features.Sales.Receipts {

    public interface IReceiptEmailSender {

        Task SendReceiptsToEmail(EmailReceiptVM model);

    }

}