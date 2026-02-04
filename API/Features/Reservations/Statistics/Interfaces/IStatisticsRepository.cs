using System.Collections.Generic;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Statistics {

    public interface IStatisticsRepository : IRepository<Reservation> {

        IEnumerable<StatisticsNationalityVM> GetPerNationality(StatisticsCriteriaVM criteria);
        IEnumerable<StatisticsVM> Get(StatisticsCriteriaVM criteria);
        IEnumerable<StatisticsVM> GetPerCustomer(StatisticsCriteriaVM criteria);
        IEnumerable<StatisticsVM> GetPerDestination(StatisticsCriteriaVM criteria);
        IEnumerable<StatisticsVM> GetPerDriver(StatisticsCriteriaVM criteria);
        IEnumerable<StatisticsVM> GetPerPort(StatisticsCriteriaVM criteria);
        IEnumerable<StatisticsVM> GetPerShip(StatisticsCriteriaVM criteria);

    }

}