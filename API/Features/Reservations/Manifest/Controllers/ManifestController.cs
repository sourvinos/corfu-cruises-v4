using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Manifest {

    [Route("api/[controller]")]
    public class ManifestController : ControllerBase {

        #region variables

        private readonly IManifestRepository repo;

        #endregion

        public ManifestController(IManifestRepository repo) {
            this.repo = repo;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public Task<IEnumerable<ManifestPassengerVM>> Post([FromBody] ManifestCriteriaVM criteria) {
            return repo.GetPassengersAsync(criteria.Date, criteria.DestinationId, criteria.PortId, criteria.ShipId);
        }

        [HttpGet("{shipId}")]
        [Authorize(Roles = "admin")]
        public Task<IEnumerable<ManifestCrewVM>> GetAsync(int shipId) {
            return repo.GetCrewAsync(shipId);
        }

    }

}