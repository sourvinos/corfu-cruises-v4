using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp;
using System.Globalization;
using System.IO;

namespace API.Features.Sales.Receipts {

    public class ReceiptPdfRepository : IReceiptPdfRepository {

        public string BuildPdf(ReceiptPdfVM receipt) {
            var locale = CultureInfo.CreateSpecificCulture("el-GR");
            GlobalFontSettings.FontResolver = new FileFontResolver();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PdfDocument document = new();
            PdfPage page = document.AddPage();
            page.Size = PageSize.A4;
            XFont logoFont = new("ACCanterBold", 20);
            XFont robotoMonoFont = new("RobotoMono", 6);
            XFont robotoMonoFontBig = new("RobotoMono", 8);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            AddLogo(gfx);
            AddIssuer(gfx, logoFont, robotoMonoFont, receipt);
            AddReceiptDetails(gfx, robotoMonoFont, receipt);
            AddCounterPart(gfx, robotoMonoFont, receipt);
            AddRemarks(gfx,robotoMonoFont,receipt);
            AddSummary(gfx, robotoMonoFont, robotoMonoFontBig, locale, receipt);
            AddBalances(gfx, robotoMonoFont, robotoMonoFontBig, locale, receipt);
            AddBankAccounts(gfx, robotoMonoFont, receipt);
            var filename = receipt.InvoiceId + ".pdf";
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + "Invoices" + Path.DirectorySeparatorChar + filename);
            document.Save(fullpathname);
            return filename;
        }

        public FileStreamResult OpenPdf(string filename) {
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + "Invoices" + Path.DirectorySeparatorChar + filename);
            byte[] byteArray = File.ReadAllBytes(fullpathname);
            MemoryStream memoryStream = new(byteArray);
            return new FileStreamResult(memoryStream, "application/pdf");
        }

        public void AddLogo(XGraphics gfx) {
            XImage image = XImage.FromFile(Path.Combine("Images" + Path.DirectorySeparatorChar + "Background.png"));
            gfx.DrawImage(image, 40, 20, 100, 100);
        }

        private static void AddIssuer(XGraphics gfx, XFont logoFont, XFont robotoMonoFont, ReceiptPdfVM receipt) {
            var top = 42;
            var left = 40;
            gfx.DrawString(receipt.Issuer.FullDescription, logoFont, XBrushes.Black, new XPoint(left += 95, top));
            gfx.DrawString(receipt.Issuer.Profession, robotoMonoFont, XBrushes.Black, new XPoint(left += 5, top += 10));
            gfx.DrawString("ΑΦΜ: " + receipt.Issuer.VatNumber, robotoMonoFont, XBrushes.Black, new XPoint(left += 5, top += 10));
            gfx.DrawString("ΔΟΥ: " + receipt.Issuer.TaxOffice, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(receipt.Issuer.Street + " " + receipt.Issuer.Number + " " + receipt.Issuer.PostalCode, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(receipt.Issuer.City, robotoMonoFont, XBrushes.Black, new XPoint(left -= 4, top += 10));
            gfx.DrawString("ΤΗΛΕΦΩΝΑ: " + receipt.Issuer.Phones, robotoMonoFont, XBrushes.Black, new XPoint(left -= 4, top += 10));
            gfx.DrawString("EMAIL: " + receipt.Issuer.Email, robotoMonoFont, XBrushes.Black, new XPoint(left -= 7, top += 10));
        }

        private static void AddReceiptDetails(XGraphics gfx, XFont robotoMonoFont, ReceiptPdfVM receipt) {
            var top = 29;
            var right = 560;
            gfx.DrawString("ΗΜΕΡΟΜΗΝΙΑ ΕΚΔΟΣΗΣ: " + DateHelpers.FormatDateStringToLocaleString(receipt.Header.Date), robotoMonoFont, XBrushes.Black, new XRect(right, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(receipt.DocumentType.Description, robotoMonoFont, XBrushes.Black, new XRect(right, top += 10, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΣΕΙΡΑ: " + receipt.DocumentType.Batch, robotoMonoFont, XBrushes.Black, new XRect(right, top += 10, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΝΟ: " + receipt.Header.InvoiceNo, robotoMonoFont, XBrushes.Black, new XRect(right, top += 10, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ: " + receipt.PaymentMethod, robotoMonoFont, XBrushes.Black, new XRect(right, top += 10, 0, 0), new() { Alignment = XStringAlignment.Far });
        }

        private static void AddCounterPart(XGraphics gfx, XFont robotoMonoFont, ReceiptPdfVM receipt) {
            var top = 150;
            var left = 40;
            gfx.DrawString("ΣΤΟΙΧΕΙΑ ΛΗΠΤΗ", robotoMonoFont, XBrushes.Black, new XRect(left, top, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawLine(XPens.LightGray, left, top += 10, 90, top);
            gfx.DrawString(receipt.Customer.FullDescription, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("ΔΡΑΣΤΗΡΙΟΤΗΤΑ: " + receipt.Customer.Profession, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("ΑΦΜ: " + receipt.Customer.VatNumber, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("ΔΟΥ: " + receipt.Customer.TaxOffice, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("ΔΙΕΥΘΥΝΣΗ: " + receipt.Customer.Street + " " + receipt.Customer.Number + ", " + receipt.Customer.PostalCode + ", " + receipt.Customer.City, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("ΤΗΛΕΦΩΝΑ: " + receipt.Customer.Phones, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("EMAIL: " + receipt.Customer.Email, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
        }

        private static void AddRemarks(XGraphics gfx, XFont robotoMonoFont, ReceiptPdfVM receipt) {
            var top = 250;
            var left = 40;
            gfx.DrawString("ΠΑΡΑΤΗΡΗΣΕΙΣ", robotoMonoFont, XBrushes.Black, new XRect(left, top, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawLine(XPens.LightGray, left, top += 10, 90, top);
            gfx.DrawString(receipt.Remarks, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
        }
 
        private static void AddSummary(XGraphics gfx, XFont robotoMonoFont, XFont robotoMonoFontBig, CultureInfo locale, ReceiptPdfVM invoice) {
            var top = 475;
            var left = 450;
            var amountsRight = 560;
            gfx.DrawString("ΣΥΝΟΛΙΚΗ ΑΞΙΑ", robotoMonoFontBig, XBrushes.Black, new XRect(left, top += 20, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("€" + invoice.Summary.GrossAmount.ToString("N2", locale), robotoMonoFontBig, XBrushes.Black, new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
        }

        private static void AddBalances(XGraphics gfx, XFont robotoMonoFont, XFont robotoMonoFontBig, CultureInfo locale, ReceiptPdfVM invoice) {
            var top = 520;
            var left = 450;
            var amountsRight = 560;
            gfx.DrawString("ΥΠΟΛΟΙΠΑ", robotoMonoFont, XBrushes.Black, new XRect(left, top, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawLine(XPens.LightGray, left, top += 10, amountsRight, top);
            gfx.DrawString("ΠΡΟΗΓΟΥΜΕΝΟ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.PreviousBalance.ToString("N2", locale), robotoMonoFont, XBrushes.Black, new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΝΕΟ", robotoMonoFontBig, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.NewBalance.ToString("N2", locale), robotoMonoFontBig, XBrushes.Black, new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
        }

        private static void AddBankAccounts(XGraphics gfx, XFont robotoMonoFont, ReceiptPdfVM invoice) {
            var bottom = 820;
            var left = 40;
            foreach (var bankAccount in invoice.BankAccounts) {
                gfx.DrawString(bankAccount.Description, robotoMonoFont, XBrushes.Black, new XRect(left, bottom -= 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            }
            gfx.DrawString("ΤΡΑΠΕΖΙΚΟΙ ΛΟΓΑΡΙΑΣΜΟΙ", robotoMonoFont, XBrushes.Black, new XRect(left, bottom -= 20, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawLine(XPens.LightGray, left, bottom += 10, 119, bottom);
        }

    }

}