using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace API.Features.Sales.Invoices {

    [Route("api/[controller]")]
    public class InvoicesXmlController : ControllerBase {

        #region variables

        private readonly IInvoiceXmlRepository invoiceAadeRepo;
        private readonly IInvoiceReadRepository invoiceReadRepo;
        private readonly IMapper mapper;

        #endregion

        public InvoicesXmlController(IInvoiceXmlRepository invoiceAadeRepo, IInvoiceReadRepository invoiceReadRepo, IMapper mapper) {
            this.invoiceAadeRepo = invoiceAadeRepo;
            this.invoiceReadRepo = invoiceReadRepo;
            this.mapper = mapper;
        }

        [HttpGet("{invoiceId}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string invoiceId) {
            var x = await invoiceReadRepo.GetByIdForXmlAsync(invoiceId);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Invoice, XmlBuilderVM>(x)
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPost("uploadInvoice")]
        [Authorize(Roles = "admin")]
        public ResponseWithBody UploadInvoice([FromBody] XmlInvoiceVM invoice) {
            var response = SaveInvoicePrettyResponse(invoice.InvoiceHeader, "xmls", invoiceAadeRepo.UploadXMLAsync(XElement.Load(invoiceAadeRepo.CreateXMLFileAsync(invoice)), invoice.Credentials).Result);
            if (response.Contains("Success")) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = new {
                        invoice.InvoiceId,
                        response
                    },
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 402
                };
            }
        }

        [HttpPost("cancelInvoice")]
        [Authorize(Roles = "admin")]
        public ResponseWithBody CancelInvoice([FromBody] XmlInvoiceVM invoice) {
            var response = SaveInvoicePrettyResponse(invoice.InvoiceHeader, "xmlsCancelled", invoiceAadeRepo.CancelInvoiceAsync(invoice.Aade.Mark, invoice.Credentials).Result);
            if (response.Contains("Success")) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = new {
                        response
                    },
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 402
                };
            }
        }

        private string SaveInvoicePrettyResponse(XmlHeaderVM invoice, string subdirectory, string response) {
            return invoiceAadeRepo.SaveInvoiceResponse(invoice, subdirectory, response
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">", "")
                .Replace("</string>", "")).ToString();
        }

    }

}