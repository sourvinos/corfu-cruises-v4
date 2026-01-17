using System.Threading.Tasks;
using API.Features.Reservations.Parameters;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Linq;

namespace API.Features.Reservations.Reservations {

    public class ReservationLinkTwist : IReservationLinkTwist {

        #region variables

        private readonly IReservationParametersRepository parametersRepo;

        #endregion

        public ReservationLinkTwist(IReservationParametersRepository parametersRepo) {
            this.parametersRepo = parametersRepo;
        }

        public async Task<LinkTwistReservation> GetReservationAsync(string code) {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
            return CalculatePassengers(JsonSerializer.Deserialize<LinkTwistReservation>(await httpClient.GetStringAsync(GetParameters().APIUrl + "/bookings/" + code)));
        }

        public async Task<LinkTwistReservation[]> GetReservationsAsync(LinkTwistReservationCriteriaVM criteria) {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
            return JsonSerializer.Deserialize<LinkTwistReservation[]>(await httpClient.GetStringAsync($"{GetParameters().APIUrl}/bookings?activity_date_time_from={criteria.FromDate}&activity_date_time_to={criteria.ToDate}"));
        }

        private static LinkTwistReservation CalculatePassengers(LinkTwistReservation x) {
            x.Adults = x.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Adult"));
            x.TotalPax = x.Details.Count;
            return x;
        }

        private ReservationParametersVM GetParameters() {
            var parameters = parametersRepo.GetAsync().Result;
            return new ReservationParametersVM {
                APIKey = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoAPIKey : parameters.LinkTwistLiveAPIKey,
                APIUrl = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoUrl : parameters.LinkTwistLiveUrl
            };
        }

    }

}