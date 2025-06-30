using System.Threading.Tasks;

namespace API.Features.Sales.Receipts {

    public interface IReceiptValidation {

        Task<int> IsValidAsync(Receipt x, ReceiptWriteDto transaction);

    }

}