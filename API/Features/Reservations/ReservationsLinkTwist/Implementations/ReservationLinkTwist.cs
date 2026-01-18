using System.Threading.Tasks;
using API.Features.Reservations.Parameters;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Linq;
using API.Infrastructure.Helpers;
using System;

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
            var x = JsonSerializer.Deserialize<LinkTwistReservation>(await httpClient.GetStringAsync(GetParameters().APIUrl + "/bookings/" + code));
            x.Destination = x.Details.FirstOrDefault().Destination;
            x.Adults = x.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Adult"));
            x.Kids = x.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Child"));
            x.Free = x.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Infant"));
            x.Date = DateHelpers.DateToISOString(DateHelpers.StringToDate(x.Details.FirstOrDefault().Date));
            x.TotalPax = x.Adults + x.Kids + x.Free;
            return x;
        }

        public async Task<LinkTwistReservation[]> GetReservationsAsync(LinkTwistReservationCriteriaVM criteria) {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
            var x = JsonSerializer.Deserialize<LinkTwistReservation[]>(await httpClient.GetStringAsync($"{GetParameters().APIUrl}/bookings?activity_date_time_from={criteria.FromDate}&activity_date_time_to={criteria.ToDate}"));
            foreach (var item in x) {
                item.Date = DateHelpers.DateToISOString(DateHelpers.StringToDate(item.Details.FirstOrDefault().Date));
                item.Destination = item.Details.FirstOrDefault().Destination;
                item.Adults = item.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Adult"));
                item.Kids = item.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Child"));
                item.Free = item.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Infant"));
                item.TotalPax = item.Adults + item.Kids + item.Free;
            }
            return x;
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