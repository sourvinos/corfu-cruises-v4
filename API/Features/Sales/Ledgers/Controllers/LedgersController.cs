using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Ledgers {

    [Route("api/[controller]")]
    public class LedgersSalesController : ControllerBase {

        #region variables

        private readonly ILedgerPdfBuilder ledgerPdfBuilder;
        private readonly ILedgerSalesRepository repo;

        #endregion

        public LedgersSalesController(ILedgerPdfBuilder ledgerPdfBuilder, ILedgerSalesRepository repo) {
            this.ledgerPdfBuilder = ledgerPdfBuilder;
            this.repo = repo;
        }

        [HttpPost("getLedger")]
        [Authorize(Roles = "admin")]
        public Task<List<LedgerVM>> GetLedger([FromBody] LedgerCriteria criteria) {
            return ProcessLedger(criteria);
        }

        [HttpPost("buildPdf")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> BuildPdf([FromBody] LedgerCriteria criteria) {
            var x = await ledgerPdfBuilder.CreatePdfLedger(criteria, (int)criteria.ShipOwnerId);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = x
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpGet("[action]/{filename}")]
        [Authorize(Roles = "admin")]
        public IActionResult OpenPdf([FromRoute] string filename) {
            return ledgerPdfBuilder.OpenPdf(filename);
        }

        private async Task<List<LedgerVM>> ProcessLedger(LedgerCriteria criteria) {
            var records = repo.BuildBalanceForLedger(await repo.GetForLedger(true, DateHelpers.DateToISOString(criteria.FromDate), DateHelpers.DateToISOString(criteria.ToDate), criteria.CustomerId, criteria?.ShipOwnerId));
            var previous = repo.BuildPrevious(records, DateHelpers.DateToISOString(criteria.FromDate));
            var requested = repo.BuildRequested(records, DateHelpers.DateToISOString(criteria.FromDate));
            var total = repo.BuildTotal(records);
            return repo.MergePreviousRequestedAndTotal(previous, requested, total);
        }

    }

}