using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Reservations.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Revenues {

    [Route("api/[controller]")]
    public class RevenuesController : ControllerBase {

        #region variables

        private readonly IRevenuesRepository repo;
        private readonly ICustomerRepository customerRepo;

        #endregion

        public RevenuesController(IRevenuesRepository repo, ICustomerRepository customerRepo) {
            this.repo = repo;
            this.customerRepo = customerRepo;
        }

        [HttpPost("buildRevenues")]
        [Authorize(Roles = "admin")]
        public Task<List<RevenuesSummaryVM>> BuildRevenues([FromBody] RevenuesCriteria criteria) {
            return ProcessRevenues(criteria);
        }

        private async Task<List<RevenuesSummaryVM>> ProcessRevenues(RevenuesCriteria criteria) {
            var summaries = new List<RevenuesSummaryVM>();
            var customers = customerRepo.GetForCriteriaAsync().Result;
            foreach (var customer in customers) {
                var records = repo.BuildBalanceForRevenues(await repo.GetForRevenues(criteria.FromDate, criteria.ToDate, customer.Id, criteria.ShipOwnerId));
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