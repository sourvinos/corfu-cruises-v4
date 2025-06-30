using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using API.Features.Reservations.Boarding;
using Cases;
using Infrastructure;
using Responses;
using Xunit;

namespace Boarding {

    [Collection("Sequence")]
    public class Passengers01Get : IClassFixture<AppSettingsFixture> {

        #region variables

        private readonly AppSettingsFixture _appSettingsFixture;
        private readonly HttpClient _httpClient;
        private readonly TestHostFixture _testHostFixture = new();
        private readonly string _actionVerb = "post";
        private readonly string _baseUrl;
        private readonly string _url = "/Boarding";

        #endregion

        public Passengers01Get(AppSettingsFixture appsettings) {
            _appSettingsFixture = appsettings;
            _baseUrl = _appSettingsFixture.Configuration.GetSection("TestingEnvironment").GetSection("BaseUrl").Value;
            _httpClient = _testHostFixture.Client;
        }

        [Fact]
        public async Task Unauthorized_Not_Logged_In() {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url, _actionVerb, "", "", null);
        }

        [Fact]
        public async Task Unauthorized_Invalid_Credentials() {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url, _actionVerb, "user-does-not-exist", "not-a-valid-password", null);
        }

        [Theory]
        [ClassData(typeof(InactiveUsersCanNotLogin))]
        public async Task Unauthorized_Inactive_Users(Login login) {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url, _actionVerb, login.Username, login.Password, null);
        }

        [Theory]
        [ClassData(typeof(CreateBoardingCriteria))]
        public async Task Simple_Users_Can_Not_List(TestBoardingCriteria criteria) {
            await Forbidden.Action(_httpClient, _baseUrl, _url, _actionVerb, "simpleuser", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", criteria);
        }

        [Theory]
        [ClassData(typeof(CreateBoardingCriteria))]
        public async Task Admins_Can_List(TestBoardingCriteria criteria) {
            var actionResponse = await ListByPost.Action(_httpClient, _baseUrl, _url, "john", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", criteria);
            var records = JsonSerializer.Deserialize<BoardingFinalGroupVM>(await actionResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(70, records.TotalPax);
            Assert.Equal(68, records.EmbarkedPassengers);
            Assert.Equal(29, records.Reservations.Count());
        }

    }

}