using Cases;
using Infrastructure;
using Responses;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Users {

    [Collection("Sequence")]
    public class Users04Put : IClassFixture<AppSettingsFixture> {

        #region variables

        private readonly AppSettingsFixture _appSettingsFixture;
        private readonly HttpClient _httpClient;
        private readonly TestHostFixture _testHostFixture = new();
        private readonly string _actionVerb = "put";
        private readonly string _baseUrl;
        private readonly string _url = "/users";
        private readonly string _notFoundUrl = "/users/582ea0d8-3938-4098-a381-4721d560b142";

        #endregion

        public Users04Put(AppSettingsFixture appsettings) {
            _appSettingsFixture = appsettings;
            _baseUrl = _appSettingsFixture.Configuration.GetSection("TestingEnvironment").GetSection("BaseUrl").Value;
            _httpClient = _testHostFixture.Client;
        }

        [Theory]
        [ClassData(typeof(UpdateValidUser))]
        public async Task Unauthorized_Not_Logged_In(TestUpdateUser record) {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url, _actionVerb, "", "", record);
        }

        [Theory]
        [ClassData(typeof(UpdateValidUser))]
        public async Task Unauthorized_Invalid_Credentials(TestUpdateUser record) {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url, _actionVerb, "user-does-not-exist", "not-a-valid-password", record);
        }

        [Theory]
        [ClassData(typeof(InactiveUsersCanNotLogin))]
        public async Task Unauthorized_Inactive_Users(Login login) {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url, _actionVerb, login.Username, login.Password, null);
        }

        [Fact]
        public async Task Simple_Users_Can_Not_Update_When_Not_Found() {
            await RecordNotFound.Action(_httpClient, _baseUrl, _notFoundUrl, "simpleuser", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa");
        }

        [Theory]
        [ClassData(typeof(UpdateValidUserNotOwnRecord))]
        public async Task Simple_Users_Can_Not_Update_Not_Owned(TestUpdateUser record) {
            var actionResponse = await RecordInvalidNotSaved.Action(_httpClient, _baseUrl, _url, _actionVerb, "simpleuser", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", record);
            Assert.Equal((HttpStatusCode)record.StatusCode, actionResponse.StatusCode);
        }

        [Theory]
        [ClassData(typeof(UpdateValidUser))]
        public async Task Simple_Users_Can_Update_Own_Account_When_Valid(TestUpdateUser record) {
            await RecordSaved.Action(_httpClient, _baseUrl, _url, _actionVerb, "simpleuser", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", record);
        }

        [Fact]
        public async Task Admins_Can_Not_Update_When_Not_Found() {
            await RecordNotFound.Action(_httpClient, _baseUrl, _notFoundUrl, "john", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa");
        }


        [Theory]
        [ClassData(typeof(UpdateInvalidUser))]
        public async Task Admins_Can_Not_Update_When_Invalid(TestUpdateUser record) {
            var actionResponse = await RecordInvalidNotSaved.Action(_httpClient, _baseUrl, _url, _actionVerb, "john", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", record);
            Assert.Equal((HttpStatusCode)record.StatusCode, actionResponse.StatusCode);
        }

        [Theory]
        [ClassData(typeof(UpdateValidUser))]
        public async Task Admins_Can_Update_When_Valid(TestUpdateUser record) {
            await RecordSaved.Action(_httpClient, _baseUrl, _url, _actionVerb, "john", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", record);
        }

    }

}