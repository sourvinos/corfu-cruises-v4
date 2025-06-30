using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Availability {

    [Route("api/[controller]")]
    public class AvailabilityController : ControllerBase {

        private readonly IAvailabilityCalendar availabilityCalendar;

        public AvailabilityController(IAvailabilityCalendar availabilityCalendar) {
            this.availabilityCalendar = availabilityCalendar;
        }

        [HttpGet("fromDate/{fromDate}/toDate/{toDate}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<AvailabilityGroupVM>> CalculateAvailabilityAsync(string fromDate, string toDate) {
            return availabilityCalendar.CalculateFreePax(availabilityCalendar.GetPaxPerPort(availabilityCalendar.AddBatchId(await availabilityCalendar.GetScheduleAsync(fromDate, toDate)), await availabilityCalendar.GetReservationsAsync(fromDate, toDate)));
        }

    }

}