using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Receipts {

    public interface IReceiptRepository : IRepository<Receipt> {

        Task<IEnumerable<ReceiptListVM>> GetForPeriodAsync(ReceiptListCriteriaVM criteria);
        Task<Receipt> GetByIdAsync(string transactionId, bool includeTables);
        Task<Receipt> GetByIdForPdfAsync(string receiptId);
        Task<Receipt> GetByIdForPatchEmailSent(string receiptId);
        void UpdateEmailStatus(Receipt receipt, string receiptId, bool isEmailPending, bool isEmailSent);
        Task<int> IncreaseReceiptNoAsync(ReceiptWriteDto receipt);
        void UpdateIsCancelled(Receipt receipt, string receiptId);

    }

}