using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using API.Features.Sales.Receipts;
using Cases;
using Infrastructure;
using Responses;
using Xunit;

namespace Receipts {

    [Collection("Sequence")]
    public class Receipts01Get : IClassFixture<AppSettingsFixture> {

        #region variables

        private readonly AppSettingsFixture _appSettingsFixture;
        private readonly HttpClient _httpClient;
        private readonly TestHostFixture _testHostFixture = new();
        private readonly string _actionVerb = "get";
        private readonly string _baseUrl;
        private readonly string _url = "/receipts";

        #endregion

        public Receipts01Get(AppSettingsFixture appsettings) {
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

        [Fact]
        public async Task Simple_Users_Can_Not_List() {
            await Forbidden.Action(_httpClient, _baseUrl, _url, _actionVerb, "simpleuser", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", null);
        }

        [Fact]
        public async Task Admins_Can_List() {
            var actionResponse = await List.Action(_httpClient, _baseUrl, _url, "john", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa");
            var records = JsonSerializer.Deserialize<List<ReceiptListVM>>(await actionResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Single(records);
        }

    }

}