using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.EmailServices;

namespace API.Features.Sales.Ledgers {

    public interface ILedgerSalesRepository {

        Task<IEnumerable<LedgerVM>> GetForLedger(bool ignoreConnectedUserIdentity, string fromDate, string toDate, int customerId, int? shipOwnerId);
        IEnumerable<LedgerVM> BuildBalanceForLedger(IEnumerable<LedgerVM> records);
        LedgerVM BuildPrevious(IEnumerable<LedgerVM> records, string fromDate);
        List<LedgerVM> BuildRequested(IEnumerable<LedgerVM> records, string fromDate);
        LedgerVM BuildTotal(IEnumerable<LedgerVM> records);
        Task<IEnumerable<EmailLedgerSaleQueue>> GetFromChildTable(string entityId);
        List<LedgerVM> MergePreviousRequestedAndTotal(LedgerVM previousPeriod, List<LedgerVM> requestedPeriod, LedgerVM total);
        Task<IEnumerable<LedgerVM>> GetForBalanceAsync(int customerId);

    }

}