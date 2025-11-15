using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using API.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Reservations {

    [Route("api/[controller]")]
    public class ReservationsLinkTwistController : ControllerBase {

        public ReservationsLinkTwistController() { }

        [HttpGet("[action]")]
        [Authorize(Roles = "admin")]
        public async Task<LinkTwistReservation> GetByCode() {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("API-Key", "BBAA6B64-FCBF-4777-9E9F-64340FF6697B");
            var x = client.GetStringAsync("https://test.api.link-twist.com/bookings/8E35.3YE1WVK8U8");
            var people = JsonSerializer.Deserialize<LinkTwistReservation>(await x);
            people.Details.ForEach(x => x.Date = x.Date[..10]);
            people.IsCancelled = people.CancelledAt != null;
            people.Details.ForEach(x => x.Passenger.Birthdate = DateHelpers.LinkTwistToISOString(x.Passenger.Birthdate));
            return people;
        }

        // [HttpGet("[action]")]
        // [Authorize(Roles = "admin")]
        // public async Task<Root[]> GetByDateRange() {
        //     using HttpClient client = new();
        //     client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //     client.DefaultRequestHeaders.Add("API-Key", "BBAA6B64-FCBF-4777-9E9F-64340FF6697B");
        //     var x = await client.GetStringAsync("https://test.api.link-twist.com/bookings?activity_date_time_from=2025-11-04&activity_date_time_to=2025-11-04");
        //     // var x = await client.GetStringAsync("https://test.api.link-twist.com/bookings/8E35.3YE1WVK8U8");
        //     return JsonConvert.DeserializeObject<Root[]>(x);
        // }

    }

}