using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp;
using System.Drawing.Imaging;
using System.Drawing;
using System.Globalization;
using System.IO;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;
using ZXing;
using System.Collections.Generic;

namespace API.Features.Sales.Invoices {

    public class InvoicePdfRepository : IInvoicePdfRepository {

        public string BuildPdf(InvoicePdfVM invoice) {
            var locale = CultureInfo.CreateSpecificCulture("el-GR");
            GlobalFontSettings.FontResolver = new FileFontResolver();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PdfDocument document = new();
            PdfPage page = document.AddPage();
            page.Size = PageSize.A4;
            XFont logoFont = new("ACCanterBold", 20);
            XFont robotoMonoFont = new("RobotoMono", 7);
            XFont robotoMonoFontBig = new("RobotoMono", 8);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            AddLogo(gfx);
            AddIssuer(gfx, logoFont, robotoMonoFont, invoice);
            AddInvoiceDetails(gfx, robotoMonoFont, invoice);
            AddTripDetails(gfx, robotoMonoFont, invoice);
            AddCounterPart(gfx, robotoMonoFont, invoice);
            AddFirstPort(gfx, robotoMonoFont, locale, invoice);
            AddSecondPort(gfx, robotoMonoFont, locale, invoice);
            AddPortTotals(gfx, robotoMonoFont, locale, invoice);
            AddSummary(gfx, robotoMonoFont, robotoMonoFontBig, locale, invoice);
            AddBalances(gfx, robotoMonoFont, robotoMonoFontBig, locale, invoice);
            AddBankAccounts(gfx, robotoMonoFont, invoice);
            if (invoice.Aade.Discriminator == "aade") {
                PrintAade(gfx, robotoMonoFont, invoice.Aade);
            } else {
                PrintOxygenAade(gfx, robotoMonoFont, invoice.Aade);
            }
            var filename = invoice.InvoiceId + ".pdf";
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + "Invoices" + Path.DirectorySeparatorChar + filename);
            document.Save(fullpathname);
            return filename;
        }

        public string BuildMultiPagePdf(IEnumerable<InvoicePdfVM> invoices) {
            PdfDocument document = new();
            GlobalFontSettings.FontResolver = new FileFontResolver();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            XFont logoFont = new("ACCanterBold", 20);
            XFont robotoMonoFont = new("RobotoMono", 7);
            XFont robotoMonoFontBig = new("RobotoMono", 8);
            CultureInfo locale = CultureInfo.CreateSpecificCulture("el-GR");
            foreach (var invoice in invoices) {
                PdfPage page = document.AddPage();
                page.Size = PageSize.A4;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                AddLogo(gfx);
                AddIssuer(gfx, logoFont, robotoMonoFont, invoice);
                AddInvoiceDetails(gfx, robotoMonoFont, invoice);
                AddTripDetails(gfx, robotoMonoFont, invoice);
                AddCounterPart(gfx, robotoMonoFont, invoice);
                AddFirstPort(gfx, robotoMonoFont, locale, invoice);
                AddSecondPort(gfx, robotoMonoFont, locale, invoice);
                AddPortTotals(gfx, robotoMonoFont, locale, invoice);
                AddSummary(gfx, robotoMonoFont, robotoMonoFontBig, locale, invoice);
                AddBalances(gfx, robotoMonoFont, robotoMonoFontBig, locale, invoice);
                AddBankAccounts(gfx, robotoMonoFont, invoice);
                if (invoice.Aade.Discriminator == "aade") {
                    PrintAade(gfx, robotoMonoFont, invoice.Aade);
                } else {
                    PrintOxygenAade(gfx, robotoMonoFont, invoice.Aade);
                }
            }
            var filename = "CombinedInvoices" + ".pdf";
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

        private static void AddLogo(XGraphics gfx) {
            XImage image = XImage.FromFile(Path.Combine("Images" + Path.DirectorySeparatorChar + "Background.png"));
            gfx.DrawImage(image, 40, 20, 100, 100);
        }

        private static void AddIssuer(XGraphics gfx, XFont logoFont, XFont robotoMonoFont, InvoicePdfVM invoice) {
            var top = 42;
            var left = 40;
            gfx.DrawString(invoice.Issuer.FullDescription, logoFont, XBrushes.Black, new XPoint(left += 95, top));
            gfx.DrawString(invoice.Issuer.Profession, robotoMonoFont, XBrushes.Black, new XPoint(left += 5, top += 10));
            gfx.DrawString("ΑΦΜ: " + invoice.Issuer.VatNumber, robotoMonoFont, XBrushes.Black, new XPoint(left += 5, top += 10));
            gfx.DrawString("ΔΟΥ: " + invoice.Issuer.TaxOffice, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(invoice.Issuer.Street + " " + invoice.Issuer.Number + " " + invoice.Issuer.PostalCode, robotoMonoFont, XBrushes.Black, new XPoint(left, top += 10));
            gfx.DrawString(invoice.Issuer.City, robotoMonoFont, XBrushes.Black, new XPoint(left -= 4, top += 10));
            gfx.DrawString("ΤΗΛΕΦΩΝΑ: " + invoice.Issuer.Phones, robotoMonoFont, XBrushes.Black, new XPoint(left -= 4, top += 10));
            gfx.DrawString("EMAIL: " + invoice.Issuer.Email, robotoMonoFont, XBrushes.Black, new XPoint(left -= 7, top += 10));
        }

        private static void AddInvoiceDetails(XGraphics gfx, XFont robotoMonoFont, InvoicePdfVM invoice) {
            var top = 29;
            var right = 560;
            gfx.DrawString("ΗΜΕΡΟΜΗΝΙΑ ΕΚΔΟΣΗΣ: " + DateHelpers.FormatDateStringToLocaleString(invoice.Header.Date), robotoMonoFont, XBrushes.Black, new XRect(right, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.DocumentType.Description, robotoMonoFont, XBrushes.Black, new XRect(right, top += 10, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΣΕΙΡΑ: " + invoice.DocumentType.Batch, robotoMonoFont, XBrushes.Black, new XRect(right, top += 10, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΝΟ: " + invoice.Header.InvoiceNo, robotoMonoFont, XBrushes.Black, new XRect(right, top += 10, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ: " + invoice.PaymentMethod, robotoMonoFont, XBrushes.Black, new XRect(right, top += 10, 0, 0), new() { Alignment = XStringAlignment.Far });
        }

        private static void AddTripDetails(XGraphics gfx, XFont robotoMonoFont, InvoicePdfVM invoice) {
            var top = 82;
            var right = 560;
            gfx.DrawLine(XPens.LightGray, right - 100, top += 2, right, top);
            gfx.DrawString("ΗΜΕΡΟΜΗΝΙΑ ΕΚΔΡΟΜΗΣ: " + DateHelpers.FormatDateStringToLocaleString(invoice.Header.TripDate), robotoMonoFont, XBrushes.Black, new XRect(right, top += 5, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΠΛΟΙΟ: " + invoice.Ship.Description + " " + invoice.Ship.RegistryNo, robotoMonoFont, XBrushes.Black, new XRect(right, top += 10, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΜΕΤΑΦΟΡΑ ΑΤΟΜΩΝ ΣΕ ΕΚΔΡΟΜΗ: " + invoice.Destination, robotoMonoFont, XBrushes.Black, new XRect(right, top += 10, 0, 0), new() { Alignment = XStringAlignment.Far });
        }

        private static void AddCounterPart(XGraphics gfx, XFont robotoMonoFont, InvoicePdfVM invoice) {
            var top = 150;
            var left = 40;
            gfx.DrawString("ΣΤΟΙΧΕΙΑ ΛΗΠΤΗ", robotoMonoFont, XBrushes.Black, new XRect(left, top, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawLine(XPens.LightGray, left, top += 10, 200, top);
            gfx.DrawString(invoice.Customer.FullDescription, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("ΔΡΑΣΤΗΡΙΟΤΗΤΑ: " + invoice.Customer.Profession, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("ΑΦΜ: " + invoice.Customer.VatNumber, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("ΔΟΥ: " + invoice.Customer.TaxOffice, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("ΔΙΕΥΘΥΝΣΗ: " + invoice.Customer.Street + " " + invoice.Customer.Number + ", " + invoice.Customer.PostalCode + ", " + invoice.Customer.City, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("ΤΗΛΕΦΩΝΑ: " + invoice.Customer.Phones, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("EMAIL: " + invoice.Customer.Email, robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
        }

        private static void AddFirstPort(XGraphics gfx, XFont robotoMonoFont, CultureInfo locale, InvoicePdfVM invoice) {
            var top = 250;
            var left = 40;
            var personsRight = 114;
            var amountsRight = 150;
            var totalAmountsRight = 200;
            gfx.DrawString("CORFU PORT", robotoMonoFont, XBrushes.Black, new XRect(left, top, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawLine(XPens.LightGray, left, top += 10, totalAmountsRight, top);
            gfx.DrawString("ΕΝΗΛΙΚΕΣ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("w/TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[0].AdultsWithTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].AdultsWithTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[0].AdultsPriceWithTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].AdultsWithTransfer), new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[0].AdultsTotalAmountWithTransfer.ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[0].AdultsWithTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("w/o TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[0].AdultsWithoutTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].AdultsWithoutTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[0].AdultsPriceWithoutTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].AdultsWithoutTransfer), new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[0].AdultsTotalAmountWithoutTransfer.ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[0].AdultsWithoutTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΠΑΙΔΙΑ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("w/TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[0].KidsWithTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].KidsWithTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[0].KidsPriceWithTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].KidsWithTransfer), new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[0].KidsTotalAmountWithTransfer.ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[0].KidsWithTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("w/o TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[0].KidsWithoutTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].KidsWithoutTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[0].KidsPriceWithoutTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].KidsWithoutTransfer), new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[0].KidsTotalAmountWithoutTransfer.ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[0].KidsWithoutTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΔΩΡΕΑΝ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("w/TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[0].FreeWithTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].FreeWithTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("w/o TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[0].FreeWithoutTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].FreeWithTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΣΥΝΟΛΑ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 20, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[0].TotalPax.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].TotalPax), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[0].TotalAmount.ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[0].TotalPax), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
        }

        private static void AddSecondPort(XGraphics gfx, XFont robotoMonoFont, CultureInfo locale, InvoicePdfVM invoice) {
            var top = 250;
            var left = 250;
            var personsRight = 314;
            var amountsRight = 350;
            var totalAmountsRight = 400;
            gfx.DrawString("LEFKIMMI PORT", robotoMonoFont, XBrushes.Black, new XRect(left, top, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawLine(XPens.LightGray, left, top += 10, totalAmountsRight, top);
            gfx.DrawString("ΕΝΗΛΙΚΕΣ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("w/TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[1].AdultsWithTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[1].AdultsWithTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[1].AdultsPriceWithTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[1].AdultsWithTransfer), new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[1].AdultsTotalAmountWithTransfer.ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[1].AdultsWithTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("w/o TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[1].AdultsWithoutTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[1].AdultsWithoutTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[1].AdultsPriceWithoutTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[1].AdultsWithoutTransfer), new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[1].AdultsTotalAmountWithoutTransfer.ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[1].AdultsWithoutTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΠΑΙΔΙΑ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("w/TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[1].KidsWithTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[1].KidsWithTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[1].KidsPriceWithTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[1].KidsWithTransfer), new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[1].KidsTotalAmountWithTransfer.ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[1].KidsWithTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("w/o TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[1].KidsWithoutTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[1].KidsWithoutTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[1].KidsPriceWithoutTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[1].KidsWithoutTransfer), new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[1].KidsTotalAmountWithoutTransfer.ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[1].KidsWithoutTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΔΩΡΕΑΝ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("w/TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[1].FreeWithTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[1].FreeWithTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("w/o TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[1].FreeWithoutTransfer.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[1].FreeWithoutTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΣΥΝΟΛΑ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 20, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Ports[1].TotalPax.ToString(), robotoMonoFont, SetTextColor(invoice.Ports[1].TotalPax), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString(invoice.Ports[1].TotalAmount.ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[1].TotalPax), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
        }

        private static void AddPortTotals(XGraphics gfx, XFont robotoMonoFont, CultureInfo locale, InvoicePdfVM invoice) {
            var top = 250;
            var left = 450;
            var personsRight = 515;
            var totalAmountsRight = 560;
            gfx.DrawString("ΣΥΝΟΛΑ", robotoMonoFont, XBrushes.Black, new XRect(left, top, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawLine(XPens.LightGray, left, top += 10, totalAmountsRight, top);
            gfx.DrawString("ΕΝΗΛΙΚΕΣ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("w/TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString((invoice.Ports[0].AdultsWithTransfer + invoice.Ports[1].AdultsWithTransfer).ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].AdultsWithTransfer + invoice.Ports[1].AdultsWithTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString((invoice.Ports[0].AdultsTotalAmountWithTransfer + invoice.Ports[1].AdultsTotalAmountWithTransfer).ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[0].AdultsWithTransfer + invoice.Ports[1].AdultsWithTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("w/o TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString((invoice.Ports[0].AdultsWithoutTransfer + invoice.Ports[1].AdultsWithoutTransfer).ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].AdultsWithoutTransfer + invoice.Ports[1].AdultsWithoutTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString((invoice.Ports[0].AdultsTotalAmountWithoutTransfer + invoice.Ports[1].AdultsTotalAmountWithoutTransfer).ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[0].AdultsWithoutTransfer + invoice.Ports[1].AdultsWithoutTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΠΑΙΔΙΑ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("w/o TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString((invoice.Ports[0].KidsWithTransfer + invoice.Ports[1].KidsWithTransfer).ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].KidsWithTransfer + invoice.Ports[1].KidsWithTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString((invoice.Ports[0].KidsTotalAmountWithTransfer + invoice.Ports[1].KidsTotalAmountWithTransfer).ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[0].KidsWithTransfer + invoice.Ports[1].KidsWithTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("w/o TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString((invoice.Ports[0].KidsWithoutTransfer + invoice.Ports[1].KidsWithoutTransfer).ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].KidsWithoutTransfer + invoice.Ports[1].KidsWithoutTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString((invoice.Ports[0].KidsTotalAmountWithoutTransfer + invoice.Ports[1].KidsTotalAmountWithoutTransfer).ToString("N2", locale), robotoMonoFont, SetTextColor(invoice.Ports[0].KidsWithoutTransfer + invoice.Ports[1].KidsWithoutTransfer), new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΔΩΡΕΑΝ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("w/o TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString((invoice.Ports[0].FreeWithTransfer + invoice.Ports[1].FreeWithTransfer).ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].FreeWithTransfer + invoice.Ports[1].FreeWithTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("w/o TRANSFER", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString((invoice.Ports[0].FreeWithoutTransfer + invoice.Ports[1].FreeWithoutTransfer).ToString(), robotoMonoFont, SetTextColor(invoice.Ports[0].FreeWithoutTransfer + invoice.Ports[1].FreeWithoutTransfer), new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΣΥΝΟΛΑ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 20, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString((invoice.Ports[0].TotalPax + invoice.Ports[1].TotalPax).ToString(), robotoMonoFont, XBrushes.Black, new XRect(personsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString((invoice.Ports[0].TotalAmount + invoice.Ports[1].TotalAmount).ToString("N2", locale), robotoMonoFont, XBrushes.Black, new XRect(totalAmountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
        }

        private static void AddSummary(XGraphics gfx, XFont robotoMonoFont, XFont robotoMonoFontBig, CultureInfo locale, InvoicePdfVM invoice) {
            var top = 420;
            var left = 450;
            var amountsRight = 560;
            gfx.DrawString("ΣΥΝΟΛΑ ΠΑΡΑΣΤΑΤΙΚΟΥ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 20, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawLine(XPens.LightGray, left, top += 10, amountsRight, top);
            gfx.DrawString("ΚΑΘΑΡΗ ΑΞΙΑ", robotoMonoFont, XBrushes.Black, new XRect(left, top += 15, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Summary.NetAmount.ToString("N2", locale), robotoMonoFont, XBrushes.Black, new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΦΠΑ " + invoice.Summary.VatPercent + "%", robotoMonoFont, XBrushes.Black, new XRect(left, top += 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString(invoice.Summary.VatAmount.ToString("N2", locale), robotoMonoFont, XBrushes.Black, new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
            gfx.DrawString("ΣΥΝΟΛΙΚΗ ΑΞΙΑ", robotoMonoFontBig, XBrushes.Black, new XRect(left, top += 20, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawString("€" + invoice.Summary.GrossAmount.ToString("N2", locale), robotoMonoFontBig, XBrushes.Black, new XRect(amountsRight, top, 0, 0), new() { Alignment = XStringAlignment.Far });
        }

        private static void AddBalances(XGraphics gfx, XFont robotoMonoFont, XFont robotoMonoFontBig, CultureInfo locale, InvoicePdfVM invoice) {
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

        private static void PrintAade(XGraphics gfx, XFont font, InvoicePdfAadeVM aade) {
            var bottom = 810;
            var right = 560;
            gfx.DrawString("MAPK " + aade.Mark, font, XBrushes.Black, new XRect(right, bottom - 70, 0, 0), new() { Alignment = XStringAlignment.Far });
            if (aade.Url != "") { gfx.DrawImage(AddQrCode(aade.Url), 500, 749, 60, 60); }
            gfx.DrawString("UID " + aade.UId, font, XBrushes.Black, new XRect(right, bottom, 0, 0), new() { Alignment = XStringAlignment.Far });
        }

        private static void PrintOxygenAade(XGraphics gfx, XFont font, InvoicePdfAadeVM aade) {
            var bottom = 810;
            var right = 560;
            gfx.DrawString(aade.ProviderLabel, font, XBrushes.Black, new XRect(right, bottom, 0, 0), new() { Alignment = XStringAlignment.Far });
            if (aade.AuthenticationCode != "") { gfx.DrawString("Authentication Code " + aade.AuthenticationCode, font, XBrushes.Black, new XRect(right, bottom -= 10, 0, 0), new() { Alignment = XStringAlignment.Far }); }
            if (aade.Url != "") { gfx.DrawImage(AddQrCode(aade.Url), 500, 740, 60, 60); }
            if (aade.Mark != "") { gfx.DrawString("MAPK " + aade.Mark, font, XBrushes.Black, new XRect(right, bottom -= 70, 0, 0), new() { Alignment = XStringAlignment.Far }); }
            if (aade.UId != "") { gfx.DrawString("UID " + aade.UId, font, XBrushes.Black, new XRect(right, bottom -= 10, 0, 0), new() { Alignment = XStringAlignment.Far }); }
            if (aade.UId == "" && aade.Mark == "") { gfx.DrawString("Απώλεια Διασύνδεσης Επιχείρησης – Παρόχου - Transmission Failure_1", font, XBrushes.Black, new XRect(right, bottom -= 10, 0, 0), new() { Alignment = XStringAlignment.Far }); }
            if (aade.UId != "" && aade.Mark == "") { gfx.DrawString("Απώλεια Διασύνδεσης Παρόχου - ΑΑΔΕ - Transmission Failure_2", font, XBrushes.Black, new XRect(right, bottom -= 10, 0, 0), new() { Alignment = XStringAlignment.Far }); }
        }

        private static void AddBankAccounts(XGraphics gfx, XFont robotoMonoFont, InvoicePdfVM invoice) {
            var bottom = 820;
            var left = 40;
            foreach (var bankAccount in invoice.BankAccounts) {
                gfx.DrawString(bankAccount.Description, robotoMonoFont, XBrushes.Black, new XRect(left, bottom -= 10, 0, 0), new() { Alignment = XStringAlignment.Near });
            }
            gfx.DrawString("ΤΡΑΠΕΖΙΚΟΙ ΛΟΓΑΡΙΑΣΜΟΙ", robotoMonoFont, XBrushes.Black, new XRect(left, bottom -= 20, 0, 0), new() { Alignment = XStringAlignment.Near });
            gfx.DrawLine(XPens.LightGray, left, bottom += 10, 119, bottom);
        }

        private static XImage AddQrCode(string url) {
            QrCodeEncodingOptions options = new() { DisableECI = true, CharacterSet = "UTF-8" };
            BarcodeWriter writer = new() { Format = BarcodeFormat.QR_CODE, Options = options };
            Bitmap qrCodeBitmap = writer.Write(url);
            MemoryStream strm = new();
            qrCodeBitmap.Save(strm, ImageFormat.Png);
            return XImage.FromStream(strm);
        }

        private static XSolidBrush SetTextColor(int value) {
            return value == 0 ? XBrushes.LightGray : XBrushes.Black;
        }

    }

}