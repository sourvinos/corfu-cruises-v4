using API.Infrastructure.Interfaces;

namespace API.Features.Sales.BankAccounts {

    public interface IBankAccountValidation : IRepository<BankAccount> {

        int IsValid(BankAccount x, BankAccountWriteDto bankAccount);

    }

}