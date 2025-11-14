using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using Infrastructure;
using Responses;
using Xunit;

namespace Destinations {

    [Collection("Sequence")]
    public class Destinations02GetForBrowser : IClassFixture<AppSettingsFixture> {

        #region variables

        private readonly AppSettingsFixture _appSettingsFixture;
        private readonly HttpClient _httpClient;
        private readonly TestHostFixture _testHostFixture = new();
        private readonly string _baseUrl;
        private readonly string _url = "/destinations/getForBrowser";

        #endregion

        public Destinations02GetForBrowser(AppSettingsFixture appsettings) {
            _appSettingsFixture = appsettings;
            _baseUrl = _appSettingsFixture.Configuration.GetSection("TestingEnvironment").GetSection("BaseUrl").Value;
            _httpClient = _testHostFixture.Client;
        }

        [Fact]
        public async Task Get_Active() {
            var actionResponse = await List.NoAuthAction(_httpClient, _baseUrl, _url);
            var records = JsonSerializer.Deserialize<List<SimpleEntity>>(await actionResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(7, records.Count);
        }

    }

}