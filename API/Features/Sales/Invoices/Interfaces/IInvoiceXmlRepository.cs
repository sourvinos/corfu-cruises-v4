using System.Threading.Tasks;
using System.Xml.Linq;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceXmlRepository {

        string CreateXMLFileAsync(XmlInvoiceVM invoice);
        Task<string> UploadXMLAsync(XElement invoice, XmlCredentialsVM credentials);
        Task<string> CancelInvoiceAsync(string mark, XmlCredentialsVM credentials);
        string SaveInvoiceResponse(XmlHeaderVM invoiceHeader, string subdirectory, string response);

    }

}