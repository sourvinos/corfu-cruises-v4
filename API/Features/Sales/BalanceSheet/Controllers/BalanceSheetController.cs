using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Reservations.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.BalanceSheet {

    [Route("api/[controller]")]
    public class BalanceSheetController : ControllerBase {

        #region variables

        private readonly IBalanceSheetRepository repo;
        private readonly ICustomerRepository customerRepo;

        #endregion

        public BalanceSheetController(IBalanceSheetRepository repo, ICustomerRepository customerRepo) {
            this.repo = repo;
            this.customerRepo = customerRepo;
        }

        [HttpPost("buildBalanceSheet")]
        [Authorize(Roles = "admin")]
        public Task<List<BalanceSheetSummaryVM>> BuildBalanceSheet([FromBody] BalanceSheetCriteria criteria) {
            return ProcessBalanceSheet(criteria);
        }

        private async Task<List<BalanceSheetSummaryVM>> ProcessBalanceSheet(BalanceSheetCriteria criteria) {
            var summaries = new List<BalanceSheetSummaryVM>();
            var customers = customerRepo.GetForBalanceSheetAsync().Result;
            foreach (var customer in customers) {
                var records = repo.BuildBalanceForBalanceSheet(await repo.GetForBalanceSheet(criteria.FromDate, criteria.ToDate, customer.Id, criteria.ShipOwnerId));
                var previous = repo.BuildPrevious(customer, records, criteria.FromDate);
                var requested = repo.BuildRequested(customer, records, criteria.FromDate);
                var total = repo.BuildTotal(customer, records);
                var merged = repo.MergePreviousRequestedAndTotal(previous, requested, total);
                var summary = repo.Summarize(customer, merged);
                summaries.Add(summary);
            }
            return summaries;
        }

    }

}