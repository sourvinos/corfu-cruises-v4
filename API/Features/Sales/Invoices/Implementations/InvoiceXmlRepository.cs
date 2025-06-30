using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Net.Http;
using System.Xml.Serialization;
using System.Threading.Tasks;
using API.Infrastructure.Helpers;

namespace API.Features.Sales.Invoices {

    public class InvoiceXmlRepository : IInvoiceXmlRepository {

        public string CreateXMLFileAsync(XmlInvoiceVM invoice) {
            var fullpathname = FileSystemHelpers.CreateInvoiceFullPathName(invoice.InvoiceHeader, "Xmls", "invoice");
            using StringWriter sw = new();
            using XmlTextWriter xtw = new(fullpathname, null);
            xtw.Namespaces = false;
            xtw.Formatting = Formatting.Indented;
            xtw.WriteStartElement("InvoicesDoc");
            xtw.WriteAttributeString("xmlns", "http://www.aade.gr/myDATA/invoice/v1.0");
            xtw.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xtw.WriteAttributeString("xmlns:icls", "https://www.aade.gr/myDATA/incomeClassificaton/v1.0");
            xtw.WriteAttributeString("xmlns:ecls", "https://www.aade.gr/myDATA/expensesClassificaton/v1.0");
            xtw.WriteAttributeString("xsi:schemaLocation", "http://www.aade.gr/myDATA/invoice/v1.0/InvoicesDoc-v0.6.xsd");
            xtw.WriteStartElement("invoice");
            xtw.WriteStartElement("issuer");
            xtw.WriteElementString("vatNumber", invoice.Issuer.VatNumber);
            xtw.WriteElementString("country", invoice.Issuer.Country);
            xtw.WriteElementString("branch", invoice.Issuer.Branch.ToString());
            xtw.WriteEndElement();
            xtw.WriteStartElement("counterpart");
            xtw.WriteElementString("vatNumber", invoice.CounterPart.VatNumber);
            xtw.WriteElementString("country", invoice.CounterPart.Country);
            xtw.WriteElementString("branch", invoice.CounterPart.Branch.ToString());
            if (invoice.CounterPart.Country != "GR") {
                xtw.WriteElementString("name", invoice.CounterPart.Name);
            }
            xtw.WriteStartElement("address");
            xtw.WriteElementString("postalCode", invoice.CounterPart.Address.PostalCode);
            xtw.WriteElementString("city", invoice.CounterPart.Address.City);
            xtw.WriteEndElement();
            xtw.WriteEndElement();
            xtw.WriteStartElement("invoiceHeader");
            xtw.WriteElementString("series", invoice.InvoiceHeader.Series);
            xtw.WriteElementString("aa", invoice.InvoiceHeader.Aa);
            xtw.WriteElementString("issueDate", invoice.InvoiceHeader.IssueDate);
            xtw.WriteElementString("invoiceType", invoice.InvoiceHeader.InvoiceType);
            xtw.WriteElementString("currency", invoice.InvoiceHeader.Currency);
            xtw.WriteEndElement();
            xtw.WriteStartElement("paymentMethods");
            xtw.WriteStartElement("paymentMethodDetails");
            xtw.WriteElementString("type", invoice.PaymentMethod.PaymentMethodDetail.Type.ToString());
            xtw.WriteElementString("amount", invoice.PaymentMethod.PaymentMethodDetail.Amount.ToString());
            xtw.WriteEndElement();
            xtw.WriteEndElement();
            xtw.WriteStartElement("invoiceDetails");
            xtw.WriteElementString("lineNumber", invoice.InvoiceDetail.LineNumber.ToString());
            xtw.WriteElementString("netValue", invoice.InvoiceDetail.NetValue.ToString());
            xtw.WriteElementString("vatCategory", invoice.InvoiceDetail.VatCategory.ToString());
            xtw.WriteElementString("vatAmount", invoice.InvoiceDetail.VatAmount.ToString());
            if (invoice.InvoiceDetail.VatCategory == 7) {
                xtw.WriteElementString("vatExemptionCategory", invoice.InvoiceDetail.VatExemptionCategory.ToString());
            }
            xtw.WriteStartElement("incomeClassification");
            xtw.WriteElementString("icls:classificationType", invoice.InvoiceSummary.IncomeClassification.ClassificationType);
            xtw.WriteElementString("icls:classificationCategory", invoice.InvoiceSummary.IncomeClassification.ClassificationCategory);
            xtw.WriteElementString("icls:amount", invoice.InvoiceSummary.IncomeClassification.Amount.ToString());
            xtw.WriteEndElement();
            xtw.WriteEndElement();
            xtw.WriteStartElement("invoiceSummary");
            xtw.WriteElementString("totalNetValue", invoice.InvoiceSummary.TotalNetValue.ToString());
            xtw.WriteElementString("totalVatAmount", invoice.InvoiceSummary.TotalVatAmount.ToString());
            xtw.WriteElementString("totalWithheldAmount", invoice.InvoiceSummary.TotalWithheldAmount.ToString());
            xtw.WriteElementString("totalFeesAmount", invoice.InvoiceSummary.TotalFeesAmount.ToString());
            xtw.WriteElementString("totalStampDutyAmount", invoice.InvoiceSummary.TotalStampDutyAmount.ToString());
            xtw.WriteElementString("totalOtherTaxesAmount", invoice.InvoiceSummary.TotalOtherTaxesAmount.ToString());
            xtw.WriteElementString("totalDeductionsAmount", invoice.InvoiceSummary.TotalDeductionsAmount.ToString());
            xtw.WriteElementString("totalGrossValue", invoice.InvoiceSummary.TotalGrossValue.ToString());
            xtw.WriteStartElement("incomeClassification");
            xtw.WriteElementString("icls:classificationType", invoice.InvoiceSummary.IncomeClassification.ClassificationType);
            xtw.WriteElementString("icls:classificationCategory", invoice.InvoiceSummary.IncomeClassification.ClassificationCategory);
            xtw.WriteElementString("icls:amount", invoice.InvoiceSummary.IncomeClassification.Amount.ToString());
            xtw.WriteEndElement();
            xtw.WriteEndElement();
            xtw.WriteEndElement();
            xtw.WriteEndElement();
            xtw.Close();
            return fullpathname;
        }

        public async Task<string> UploadXMLAsync(XElement invoice, XmlCredentialsVM credentials) {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("aade-user-id", credentials.Username);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", credentials.SubscriptionKey);
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            string xml = "";
            XmlSerializer serializer = new(typeof(XElement));
            await using (var stringWriter = new StringWriter()) {
                await using XmlWriter writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { Async = true });
                serializer.Serialize(writer, invoice);
                xml = stringWriter.ToString();
            }
            HttpContent body = new StringContent(xml, Encoding.UTF8, "application/xml");
            HttpResponseMessage response = client.PostAsync(credentials.Url + "/SendInvoices", body).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CancelInvoiceAsync(string mark, XmlCredentialsVM credentials) {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("aade-user-id", credentials.Username);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", credentials.SubscriptionKey);
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            HttpResponseMessage response = await client.PostAsync(credentials.Url + "/CancelInvoice?mark=" + mark, null);
            return await response.Content.ReadAsStringAsync();
        }

        public string SaveInvoiceResponse(XmlHeaderVM invoiceHeader, string subdirectory, string response) {
            using StreamWriter outputFile = new(FileSystemHelpers.CreateInvoiceFullPathName(invoiceHeader, subdirectory, "response"));
            outputFile.Write(response);
            return response;
        }

    }

}