using System.Threading.Tasks;
using API.Features.Reservations.ShipOwners;
using Newtonsoft.Json.Linq;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceJsonRepository {

        JsonInvoiceVM CreateJsonInvoice(Invoice x);
        string SaveJsonInvoice(JsonInvoiceVM x);
        Task<string> UploadJsonInvoiceAsync(string x, ShipOwner z);
        JObject ShowResponseAfterUploadJsonInvoice(string x);
        string SaveInvoiceJsonResponse(JsonInvoiceVM invoice, string subdirectory, string response);

    }

}