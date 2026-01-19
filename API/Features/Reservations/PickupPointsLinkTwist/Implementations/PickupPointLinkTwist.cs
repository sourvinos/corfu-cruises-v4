using System.Threading.Tasks;
using API.Features.Reservations.Parameters;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using API.Features.Reservations.Reservations;

namespace API.Features.Reservations.PickupPointsLinkTwist {

    public class PickupPointLinkTwist : IPickupPointLinkTwist {

        #region variables

        private readonly IReservationParametersRepository parametersRepo;

        #endregion

        public PickupPointLinkTwist(IReservationParametersRepository parametersRepo) {
            this.parametersRepo = parametersRepo;
        }

        public async Task<PickupPointLinkTwistVM[]> GetAllAsync() {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
            return JsonSerializer.Deserialize<PickupPointLinkTwistVM[]>(await httpClient.GetStringAsync($"{GetParameters().APIUrl}/extras"));
        }

        public async Task<PickupPointLinkTwistVM> GetByAliasAsync(string alias) {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
            return JsonSerializer.Deserialize<PickupPointLinkTwistVM>(await httpClient.GetStringAsync(GetParameters().APIUrl + "/extras/" + alias));
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