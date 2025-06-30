using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Reservations.Schedules;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Availability {

    public interface IAvailabilityCalendar : IRepository<Schedule> {

        Task<IEnumerable<ReservationVM>> GetReservationsAsync(string fromDate, string toDate);
        Task<IEnumerable<AvailabilityGroupVM>> GetScheduleAsync(string fromDate, string toDate);
        IEnumerable<AvailabilityGroupVM> AddBatchId(IEnumerable<AvailabilityGroupVM> schedule);
        IEnumerable<AvailabilityGroupVM> GetPaxPerPort(IEnumerable<AvailabilityGroupVM> schedule, IEnumerable<ReservationVM> reservations);
        IEnumerable<AvailabilityGroupVM> CalculateFreePax(IEnumerable<AvailabilityGroupVM> schedules);

    }

}