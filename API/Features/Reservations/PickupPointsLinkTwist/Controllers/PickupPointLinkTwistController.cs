using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.PickupPointsLinkTwist {

    [Route("api/[controller]")]
    public class PickupPointsLinkTwistController : ControllerBase {

        #region variables

        private readonly IPickupPointLinkTwist linkTwist;

        #endregion

        public PickupPointsLinkTwistController(IPickupPointLinkTwist linkTwist) {
            this.linkTwist = linkTwist;
        }

        [HttpGet()]
        [Authorize(Roles = "admin")]
        public async Task<PickupPointLinkTwistVM[]> Get() {
            return await linkTwist.GetAllAsync();
        }

        [HttpGet("{alias}")]
        [Authorize(Roles = "admin")]
        public async Task<PickupPointLinkTwistVM> GetByAlias(string alias) {
            return await linkTwist.GetByAliasAsync(alias);
        }

    }

}