using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Receipts {

    public interface IReceiptCalculateBalanceRepo : IRepository<Receipt> {

        ReceiptBalanceVM CalculateBalances(ReceiptWriteDto receipt, int customerId, int shipOwnerId);
        ReceiptWriteDto AttachBalancesToCreateDto(ReceiptWriteDto receipt, ReceiptBalanceVM balances);
        decimal CalculatePreviousBalance(int customerId, int shipOwnerId);

    }

}