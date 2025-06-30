using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Reservations.Customers;

namespace API.Features.Sales.BalanceSheet {

    public interface IBalanceSheetRepository {

        Task<IEnumerable<BalanceSheetVM>> GetForBalanceSheet(string fromDate, string toDate, int customerId, int? shipOwnerId);
        IEnumerable<BalanceSheetVM> BuildBalanceForBalanceSheet(IEnumerable<BalanceSheetVM> records);
        BalanceSheetVM BuildPrevious(CustomerListVM customer, IEnumerable<BalanceSheetVM> records, string fromDate);
        List<BalanceSheetVM> BuildRequested(CustomerListVM customer, IEnumerable<BalanceSheetVM> records, string fromDate);
        BalanceSheetVM BuildTotal(CustomerListVM customer, IEnumerable<BalanceSheetVM> records);
        List<BalanceSheetVM> MergePreviousRequestedAndTotal(BalanceSheetVM previousPeriod, List<BalanceSheetVM> requestedPeriod, BalanceSheetVM total);
        Task<IEnumerable<BalanceSheetVM>> GetForBalanceAsync(int customerId);
        BalanceSheetSummaryVM Summarize(CustomerListVM customer, IEnumerable<BalanceSheetVM> ledger);
    }

}