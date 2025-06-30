using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Customers {

    [Route("api/[controller]")]
    public class CustomersAadeController : ControllerBase {

        #region variables

        private readonly ICustomerAadeRepository repo;

        #endregion

        public CustomersAadeController(ICustomerAadeRepository repo) {
            this.repo = repo;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "admin")]
        public ResponseWithBody SearchRegistry([FromBody] CustomerAadeVM vm) {
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Body = vm.VatNumber,
                Message = repo.GetResponse(vm)
            };
        }

    }

}