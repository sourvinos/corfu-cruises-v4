using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using API.Features.Reservations.Nationalities;
using Infrastructure;
using Responses;
using Xunit;

namespace Nationalities {

    [Collection("Sequence")]
    public class Nationalities02GetForBrowser : IClassFixture<AppSettingsFixture> {

        #region variables

        private readonly AppSettingsFixture _appSettingsFixture;
        private readonly HttpClient _httpClient;
        private readonly TestHostFixture _testHostFixture = new();
        private readonly string _baseUrl;
        private readonly string _url = "/nationalities/getForBrowser";

        #endregion

        public Nationalities02GetForBrowser(AppSettingsFixture appsettings) {
            _appSettingsFixture = appsettings;
            _baseUrl = _appSettingsFixture.Configuration.GetSection("TestingEnvironment").GetSection("BaseUrl").Value;
            _httpClient = _testHostFixture.Client;
        }

        [Fact]
        public async Task Get_Active() {
            var actionResponse = await List.NoAuthAction(_httpClient, _baseUrl, _url);
            var records = JsonSerializer.Deserialize<List<NationalityBrowserVM>>(await actionResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(250, records.Count);
        }

    }

}