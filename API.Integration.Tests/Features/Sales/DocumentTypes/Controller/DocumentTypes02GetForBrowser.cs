﻿using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using API.Features.Sales.DocumentTypes;
using Cases;
using Infrastructure;
using Responses;
using Xunit;

namespace DocumentTypes {

    [Collection("Sequence")]
    public class Codes02GetForBrowser : IClassFixture<AppSettingsFixture> {

        #region variables

        private readonly AppSettingsFixture _appSettingsFixture;
        private readonly HttpClient _httpClient;
        private readonly TestHostFixture _testHostFixture = new();
        private readonly string _actionVerb = "get";
        private readonly string _baseUrl;
        private readonly string _url_invoice = "/documentTypes/getForBrowserInvoice";
        private readonly string _url_receipt = "/documentTypes/getForBrowserReceipt";

        #endregion

        public Codes02GetForBrowser(AppSettingsFixture appsettings) {
            _appSettingsFixture = appsettings;
            _baseUrl = _appSettingsFixture.Configuration.GetSection("TestingEnvironment").GetSection("BaseUrl").Value;
            _httpClient = _testHostFixture.Client;
        }

        [Fact]
        public async Task Unauthorized_Not_Logged_In() {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url_invoice, _actionVerb, "", "", null);
        }

        [Fact]
        public async Task Unauthorized_Invalid_Credentials() {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url_invoice, _actionVerb, "user-does-not-exist", "not-a-valid-password", null);
        }

        [Theory]
        [ClassData(typeof(InactiveUsersCanNotLogin))]
        public async Task Unauthorized_Inactive_Users(Login login) {
            await InvalidCredentials.Action(_httpClient, _baseUrl, _url_invoice, _actionVerb, login.Username, login.Password, null);
        }

        [Theory]
        [ClassData(typeof(ActiveUsersCanLogin))]
        public async Task Active_Users_Can_Get_Active_Invoice(Login login) {
            var actionResponse = await List.Action(_httpClient, _baseUrl, _url_invoice, login.Username, login.Password);
            var records = JsonSerializer.Deserialize<List<DocumentTypeBrowserVM>>(await actionResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(4, records.Count);
        }

        [Theory]
        [ClassData(typeof(ActiveUsersCanLogin))]
        public async Task Active_Users_Can_Get_Active_Receipt(Login login) {
            var actionResponse = await List.Action(_httpClient, _baseUrl, _url_receipt, login.Username, login.Password);
            var records = JsonSerializer.Deserialize<List<DocumentTypeBrowserVM>>(await actionResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Single(records);
        }

    }

}