using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using API.Features.Reservations.ShipOwners;
using API.Infrastructure.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Features.Sales.Invoices {

    public class InvoiceJsonRepository : IInvoiceJsonRepository {

        public JsonInvoiceVM CreateJsonInvoice(Invoice invoice) {
            var x = new JsonInvoiceVM {
                Issuer = new() {
                    Vat_Number = invoice.Ship.ShipOwner.VatNumber,
                    Branch_Code = invoice.Customer.Branch.ToString()
                },
                CounterPart = new() {
                    Vat_Number = invoice.Customer.VatNumber,
                    Branch_Code = invoice.Customer.Branch.ToString(),
                    Country_Code = invoice.Customer.Nationality.Code,
                    Name = invoice.Customer.Description,
                    Address = new() {
                        Street = invoice.Customer.Street,
                        Number = invoice.Customer.Number,
                        Postal_Code = invoice.Customer.PostalCode,
                        City = invoice.Customer.City
                    }
                },
                Header = new() {
                    Series = invoice.DocumentType.Batch,
                    Number = invoice.InvoiceNo,
                    Issued_At = DateHelpers.DateTimeToISOStringWithZulu(DateHelpers.GetLocalDateTime()),
                    Invoice_Type = invoice.DocumentType.Table8_1
                },
                Payment_Methods = AddPaymentMethods(invoice),
                Lines = AddLines(invoice),
                Summary = AddSummary(invoice),
                Options = AddOptions(invoice)
            };
            return x;
        }

        public string SaveJsonInvoice(JsonInvoiceVM x) {
            var jsonString = JsonConvert.SerializeObject(x);
            var fullPathname = FileSystemHelpers.CreateInvoiceJsonFullPathName(x,"Jsons", "invoice");
            using StreamWriter outputFile = new(fullPathname);
            outputFile.Write(jsonString);
            return jsonString;
        }

        public async Task<string> UploadJsonInvoiceAsync(string x, ShipOwner z) {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", z.OxygenIsActive ? z.OxygenIsDemo ? z.OxygenDemoAPIKey : z.OxygenLiveAPIKey : "");
            var content = new StringContent(x, UTF8Encoding.UTF8, "application/json");
            var response = await client.PostAsync(z.OxygenIsActive ? z.OxygenIsDemo ? z.OxygenDemoUrl : z.OxygenLiveUrl : "", content);
            return await response.Content.ReadAsStringAsync();
        }

        public JObject ShowResponseAfterUploadJsonInvoice(string response) {
            return JObject.Parse(response);
        }

        private static List<JsonPaymentMethodDetailVM> AddPaymentMethods(Invoice invoice) {
            return new List<JsonPaymentMethodDetailVM>() { new() { Type = invoice.PaymentMethod.MyDataId, Amount = invoice.GrossAmount } };
        }

        private static List<JsonLineVM> AddLines(Invoice invoice) {
            var x = new List<JsonLineVM>() {
                new() {
                    Description = "Trip services",
                    Quantity = 1,
                    Unit_Price = invoice.NetAmount,
                    Net_Amount = invoice.NetAmount,
                    Vat_Category = invoice.Customer.VatPercentId,
                    Vat_Amount = invoice.VatAmount,
                    Total_Amount = invoice.GrossAmount,
                    Vat_Exemption_Reason_Code = invoice.Ship.ShipOwner.VatExemptionId.ToString(),
                    Classifications = AddClassifications(invoice)
                }
            };
            return x;
        }

        private static JsonSummaryVM AddSummary(Invoice invoice) {
            var x = new JsonSummaryVM() {
                Total_Net_Amount = invoice.NetAmount,
                Total_Vat_Amount = invoice.VatAmount,
                Total_Gross_Amount = invoice.GrossAmount,
                Classifications = AddClassifications(invoice)
            };
            return x;
        }

        private static List<JsonSummaryClassificationVM> AddClassifications(Invoice invoice) {
            var x = new List<JsonSummaryClassificationVM>() {
                new() {
                    Category = invoice.DocumentType.Table8_8,
                    Type = invoice.DocumentType.Table8_9,
                    Amount = invoice.NetAmount
                }
            };
            return x;
        }

        private static JsonOptionsVM AddOptions(Invoice invoice) {
            var x = new JsonOptionsVM() {
                Is_Peppol = false,
                Ignore_Classifications = false
            };
            return x;
        }

        public string SaveInvoiceJsonResponse(JsonInvoiceVM invoice, string subdirectory, string response) {
            using StreamWriter outputFile = new(FileSystemHelpers.CreateInvoiceJsonFullPathName(invoice, subdirectory, "response"));
            outputFile.Write(response);
            return response;
        }

    }

}