using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Features.Sales.Invoices {

    [Route("api/[controller]")]
    public class InvoicesJsonController : ControllerBase {

        #region variables

        private readonly IInvoiceJsonRepository invoiceJsonRepo;
        private readonly IInvoiceReadRepository invoiceReadRepo;

        #endregion

        public InvoicesJsonController(IInvoiceJsonRepository invoiceJsonRepo, IInvoiceReadRepository invoiceReadRepo) {
            this.invoiceJsonRepo = invoiceJsonRepo;
            this.invoiceReadRepo = invoiceReadRepo;
        }

        [HttpGet("[action]/{invoiceId}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string invoiceId) {
            var x = await invoiceReadRepo.GetByIdAsync(invoiceId, true);
            if (x != null) {
                var invoiceJson = invoiceJsonRepo.CreateJsonInvoice(x);
                var response = SaveInvoiceResponse(invoiceJson, "Jsons", invoiceJsonRepo.UploadJsonInvoiceAsync(invoiceJsonRepo.SaveJsonInvoice(invoiceJson), x.Ship.ShipOwner).Result);
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Body = new {
                        invoiceId,
                        response
                    },
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpGet("[action]/{invoiceId}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> DownloadByIdAsync(string invoiceId) {
            var x = await invoiceReadRepo.GetByIdAsync(invoiceId, true);
            if (x != null) {
                using HttpClient client = new();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", x.Ship.ShipOwner.OxygenLiveAPIKey);
                var response = await client.GetStringAsync(x.Ship.ShipOwner.OxygenLiveUrl + "/" + x.Aade.Id);
                JObject json = JObject.Parse(response);
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Body = new {
                        invoiceId,
                        json
                    },
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        private string SaveInvoiceResponse(JsonInvoiceVM invoice, string subdirectory, string response) {
            return invoiceJsonRepo.SaveInvoiceJsonResponse(invoice, subdirectory, response);
        }

    }

    public class CSharpMember {

        public int Code { get; set; }

    }

}