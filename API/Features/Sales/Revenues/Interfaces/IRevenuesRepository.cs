using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;

namespace API.Features.Sales.Revenues {

    public interface IRevenuesRepository {

        Task<IEnumerable<RevenuesVM>> GetForRevenues(string fromDate, string toDate, int customerId, int? shipOwnerId);
        IEnumerable<RevenuesVM> BuildBalanceForRevenues(IEnumerable<RevenuesVM> records);
        RevenuesVM BuildPrevious(SimpleEntity customer, IEnumerable<RevenuesVM> records, string fromDate);
        List<RevenuesVM> BuildRequested(SimpleEntity customer, IEnumerable<RevenuesVM> records, string fromDate);
        RevenuesVM BuildTotal(SimpleEntity customer, IEnumerable<RevenuesVM> records);
        List<RevenuesVM> MergePreviousRequestedAndTotal(RevenuesVM previousPeriod, List<RevenuesVM> requestedPeriod, RevenuesVM total);
        Task<IEnumerable<RevenuesVM>> GetForBalanceAsync(int customerId);
        RevenuesSummaryVM Summarize(SimpleEntity customer, IEnumerable<RevenuesVM> ledger);
    }

}