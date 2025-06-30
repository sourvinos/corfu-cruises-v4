using System.Threading.Tasks;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Boarding {

    [Route("api/[controller]")]
    public class BoardingController : ControllerBase {

        #region variables

        private readonly IBoardingRepository repo;

        #endregion

        public BoardingController(IBoardingRepository repo) {
            this.repo = repo;
        }

        [Authorize(Roles = "admin")]
        public async Task<BoardingFinalGroupVM> Post([FromBody] BoardingCriteria criteria) {
            return await repo.Get(criteria.Date, criteria.DestinationIds, criteria.PortIds, criteria.ShipIds);
        }

        [HttpPatch("embarkPassengers")]
        [Authorize(Roles = "admin")]
        public Response EmbarkPassengers([FromQuery] bool ignoreCurrentStatus, [FromQuery] int[] id) {
            repo.EmbarkPassengers(ignoreCurrentStatus, id);
            return new Response {
                Code = 200,
                Icon = Icons.Success.ToString(),
                Id = null,
                Message = ApiMessages.OK()
            };
        }

    }

}