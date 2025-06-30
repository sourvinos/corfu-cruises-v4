using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Features.Sales.Ledgers {

    public interface ILedgerSalesRepository {

        Task<IEnumerable<LedgerVM>> GetForLedger(string fromDate, string toDate, int customerId, int? shipOwnerId);
        IEnumerable<LedgerVM> BuildBalanceForLedger(IEnumerable<LedgerVM> records);
        LedgerVM BuildPrevious(IEnumerable<LedgerVM> records, string fromDate);
        List<LedgerVM> BuildRequested(IEnumerable<LedgerVM> records, string fromDate);
        LedgerVM BuildTotal(IEnumerable<LedgerVM> records);
        List<LedgerVM> MergePreviousRequestedAndTotal(LedgerVM previousPeriod, List<LedgerVM> requestedPeriod, LedgerVM total);
        Task<IEnumerable<LedgerVM>> GetForBalanceAsync(int customerId);

    }

}