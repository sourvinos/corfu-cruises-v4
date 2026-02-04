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

        [HttpPost("ytd")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> Get([FromBody] StatisticsCriteriaVM criteria) {
            return statisticsRepo.Get(criteria);
        }

        [HttpPost("customers")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerCustomer([FromBody] StatisticsCriteriaVM criteria) {
            return statisticsRepo.GetPerCustomer(criteria);
        }

        [HttpPost("destinations")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerDestination([FromBody] StatisticsCriteriaVM criteria) {
            return statisticsRepo.GetPerDestination(criteria);
        }

        [HttpPost("drivers")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerDriver([FromBody] StatisticsCriteriaVM criteria) {
            return statisticsRepo.GetPerDriver(criteria);
        }

        [HttpPost("ports")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerPort([FromBody] StatisticsCriteriaVM criteria) {
            return statisticsRepo.GetPerPort(criteria);
        }

        [HttpPost("ships")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerShips([FromBody] StatisticsCriteriaVM criteria) {
            return statisticsRepo.GetPerShip(criteria);
        }

        [HttpPost("nationalities")]
        [Authorize(Roles = "admin")]
        public IEnumerable<StatisticsVM> GetPerNationalities([FromBody] StatisticsCriteriaVM criteria) {
            return statisticsRepo.GetPerNationality(criteria);
        }

    }

}