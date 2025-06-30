using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cases;
using Infrastructure;
using Responses;
using Xunit;

namespace DocumentTypes {

    [Collection("Sequence")]
    public class Codes05Put : IClassFixture<AppSettingsFixture> {

        #region variables

        private readonly AppSettingsFixture _appSettingsFixture;
        private readonly HttpClient _httpClient;
        private readonly TestHostFixture _testHostFixture = new();
        private readonly string _actionVerb = "put";
        private readonly string _baseUrl;
        private readonly string _url = "/documentTypes";
        private readonly string _notFoundUrl = "/documentTypes/9999";

        #endregion

        public Codes05Put(AppSettingsFixture appsettings) {
            _appSettingsFixture = appsettings;
            _baseUrl = _appSettingsFixture.Configuration.GetSection("TestingEnvironment").GetSection("BaseUrl").Value;
            _httpClient = _testHostFixture.Client;
        }

        [Theory]
        [ClassData(typeof(UpdateValidDocumentType))]
        public async Task Unauthorized_Not_Logged_In(TestDocumentType record) {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url, _actionVerb, "", "", record);
        }

        [Theory]
        [ClassData(typeof(UpdateValidDocumentType))]
        public async Task Unauthorized_Invalid_Credentials(TestDocumentType record) {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url, _actionVerb, "user-does-not-exist", "not-a-valid-password", record);
        }

        [Theory]
        [ClassData(typeof(InactiveUsersCanNotLogin))]
        public async Task Unauthorized_Inactive_Users(Login login) {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url, _actionVerb, login.Username, login.Password, null);
        }

        [Theory]
        [ClassData(typeof(UpdateValidDocumentType))]
        public async Task Simple_Users_Can_Not_Update(TestDocumentType record) {
            await Forbidden.Action(_httpClient, _baseUrl, _url, _actionVerb, "simpleuser", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", record);
        }

        [Fact]
        public async Task Admins_Can_Not_Update_When_Not_Found() {
            await RecordNotFound.Action(_httpClient, _baseUrl, _notFoundUrl, "john", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa");
        }

        [Theory]
        [ClassData(typeof(UpdateInvalidDocumentType))]
        public async Task Admins_Can_Not_Update_When_Invalid(TestDocumentType record) {
            var actionResponse = await RecordInvalidNotSaved.Action(_httpClient, _baseUrl, _url, _actionVerb, "john", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", record);
            Assert.Equal((HttpStatusCode)record.StatusCode, actionResponse.StatusCode);
        }

        [Theory]
        [ClassData(typeof(UpdateValidDocumentType))]
        public async Task Admins_Can_Update_When_Valid(TestDocumentType record) {
            await RecordSaved.Action(_httpClient, _baseUrl, _url, _actionVerb, "john", "A#ba439de-446e-4eef-8c4b-833f1b3e18aa", record);
        }

    }

}