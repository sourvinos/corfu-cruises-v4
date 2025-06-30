using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.ShipOwners {

    [Route("api/[controller]")]
    public class ShipOwnersAadeController : ControllerBase {

        #region variables

        private readonly IShipOwnerAadeRepository repo;

        #endregion

        public ShipOwnersAadeController(IShipOwnerAadeRepository repo) {
            this.repo = repo;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "admin")]
        public ResponseWithBody SearchRegistry([FromBody] ShipOwnerAadeVM vm) {
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Body = vm.VatNumber,
                Message = repo.GetResponse(vm)
            };
        }

    }

}