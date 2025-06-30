using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Statistics {

    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase {

        #region variables

        private readonly IStatisticsRepository statisticsRepo;

        #endregion

        public StatisticsController(IStatisticsRepository statisticsRepo) {
            this.statisticsRepo = statisticsRepo;
        }

        [HttpGet("ytd/year/{year}")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> Get([FromRoute] int year) {
            return statisticsRepo.Get(year);
        }

        [HttpGet("customers/year/{year}")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerCustomer([FromRoute] int year) {
            return statisticsRepo.GetPerCustomer(year);
        }

        [HttpGet("destinations/year/{year}")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerDestination([FromRoute] int year) {
            return statisticsRepo.GetPerDestination(year);
        }

        [HttpGet("drivers/year/{year}")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerDriver([FromRoute] int year) {
            return statisticsRepo.GetPerDriver(year);
        }

        [HttpGet("ports/year/{year}")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerPort([FromRoute] int year) {
            return statisticsRepo.GetPerPort(year);
        }

        [HttpGet("ships/year/{year}")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerShip([FromRoute] int year) {
            return statisticsRepo.GetPerShip(year);
        }

        [HttpGet("nationalities/year/{year}")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerNationality([FromRoute] int year) {
            return statisticsRepo.GetPerNationality(year);
        }

        [HttpGet("users/year/{year}")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsUserVM> GetPerUser([FromRoute] int year) {
            return statisticsRepo.GetPerUser(year);
        }

    }

}