using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using API.Features.Reservations.Parameters;
using API.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Reservations {

    [Route("api/[controller]")]
    public class ReservationsLinkTwistController : ControllerBase {

        #region variables

        private readonly IReservationParametersRepository parametersRepo;

        #endregion

        public ReservationsLinkTwistController(IReservationParametersRepository parametersRepo) {
            this.parametersRepo = parametersRepo;
        }

        [HttpGet("[action]/{code}")]
        [Authorize(Roles = "admin")]
        public async Task<LinkTwistReservation> GetByCode(string code) {
            return await GetReservationAsync(GetParameters(), code);
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

        private ReservationParametersVM GetParameters() {
            var parameters = parametersRepo.GetAsync().Result;
            return new ReservationParametersVM {
                APIKey = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoAPIKey : parameters.LinkTwistLiveAPIKey,
                APIUrl = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoUrl : parameters.LinkTwistLiveUrl
            };
        }

        private static async Task<LinkTwistReservation> GetReservationAsync(ReservationParametersVM parameters, string code) {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("API-Key", parameters.APIKey);
            return ProcessResponse(await httpClient.GetStringAsync(parameters.APIUrl + "/bookings/" + code));
        }

        private static LinkTwistReservation ProcessResponse(string reservation) {
            var response = JsonSerializer.Deserialize<LinkTwistReservation>(reservation);
            response.Details.ForEach(x => x.Date = x.Date[..10]);
            response.IsCancelled = response.CancelledAt != null;
            response.Details.ForEach(x => x.Passenger.Birthdate = DateHelpers.LinkTwistToISOString(x.Passenger.Birthdate));
            return response;
        }

    }

}