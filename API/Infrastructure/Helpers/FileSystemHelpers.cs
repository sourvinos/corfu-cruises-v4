using System.IO;
using API.Features.Sales.Invoices;

namespace API.Infrastructure.Helpers {

    public static class FileSystemHelpers {

        public static string CreateInvoiceFullPathName(XmlHeaderVM invoice, string subdirectory, string prefix) {
            var date = invoice.IssueDate.Replace("-", "");
            var aa = invoice.Aa.PadLeft(5, '0');
            var series = invoice.Series.PadLeft(5, '0');
            var extension = ".xml";
            var filename = string.Concat(prefix, " ", date, " ", aa, " ", series, " ", DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime()).Replace(":", "-"), extension);
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + subdirectory, filename);
            return fullpathname;
        }

        public static string CreateInvoiceJsonFullPathName(JsonInvoiceVM invoice, string subdirectory, string prefix) {
            var date = invoice.Header.Issued_At[..10];
            var number = invoice.Header.Number.ToString().PadLeft(5, '0');
            var batch = invoice.Header.Series.PadLeft(5, '0');
            var extension = ".json";
            var filename = string.Concat(prefix, " ", date, " ", number, " ", batch, " ", DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime()).Replace(":", "-"), extension);
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + subdirectory, filename);
            return fullpathname;
        }

        public static string CreateResponseFullPathName(string subdirectory) {
            var filename = "aadeCustomer.xml";
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + subdirectory, filename);
            return fullpathname;
        }

    }

}
