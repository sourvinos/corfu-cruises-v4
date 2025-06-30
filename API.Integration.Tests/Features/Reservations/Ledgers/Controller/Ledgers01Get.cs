using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using API.Features.Reservations.Ledgers;
using Infrastructure;
using Responses;
using Xunit;

namespace Ledgers {

    [Collection("Sequence")]
    public class Ledgers : IClassFixture<AppSettingsFixture> {

        #region variables

        private readonly AppSettingsFixture _appSettingsFixture;
        private readonly HttpClient _httpClient;
        private readonly TestHostFixture _testHostFixture = new();
        private readonly string _actionVerb = "post";
        private readonly string _baseUrl;
        private readonly string _url = "/ledgersreservations";

        #endregion

        public Ledgers(AppSettingsFixture appsettings) {
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
        [ClassData(typeof(CreateAdminCriteria))]
        public async Task Admins_Can_List_From_All_Users(TestLedgerCriteria criteria) {
            var actionResponse = await ListByPost.Action(_httpClient, _baseUrl, _url, "john", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", criteria);
            var records = JsonSerializer.Deserialize<IEnumerable<LedgerVM>>(await actionResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(3, records.Count());
            Assert.Equal(41, records.First().Reservations.Sum(x => x.TotalPax));
            Assert.Equal(7, records.Last().Reservations.Sum(x => x.TotalPax));
        }

        [Theory]
        [ClassData(typeof(CreateSimpleUserCriteria))]
        public async Task Simple_Users_Can_List_Only_Owned(TestLedgerCriteria criteria) {
            var actionResponse = await ListByPost.Action(_httpClient, _baseUrl, _url, "simpleuser", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", criteria);
            var records = JsonSerializer.Deserialize<IEnumerable<LedgerVM>>(await actionResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Single(records);
            Assert.Equal(23, records.First().Reservations.Sum(x => x.TotalPax));
        }

    }

}