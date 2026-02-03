using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.LinkTwist {

    [Route("api/[controller]")]
    public class ReservationsLinkTwistController : ControllerBase {

        #region variables

        private readonly IReservationLinkTwist linkTwist;

        #endregion

        public ReservationsLinkTwistController(IReservationLinkTwist linkTwist) {
            this.linkTwist = linkTwist;
        }

        [HttpGet("{code}")]
        [Authorize(Roles = "admin")]
        public async Task<LinkTwistReservation> GetByCode(string code) {
            return await linkTwist.GetReservationAsync(code);
        }

        [HttpPost()]
        [Authorize(Roles = "admin")]
        public async Task<LinkTwistReservation[]> GetByDateRange([FromBody] LinkTwistReservationCriteriaVM criteria) {
            return await linkTwist.GetReservationsAsync(criteria);
        }

    }

}