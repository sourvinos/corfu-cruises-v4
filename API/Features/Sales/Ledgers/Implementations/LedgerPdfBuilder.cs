using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.EmailServices;
using API.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace API.Features.Sales.Ledgers {

    public class LedgerPdfBuilder : ILedgerPdfBuilder {

        private readonly ILedgerSalesRepository ledgerSaleRepo;

        public LedgerPdfBuilder(ILedgerSalesRepository ledgerSaleRepo) {
            this.ledgerSaleRepo = ledgerSaleRepo;
        }

        public FileStreamResult OpenPdf(string filename) {
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + "Ledgers" + Path.DirectorySeparatorChar + filename);
            byte[] byteArray = File.ReadAllBytes(fullpathname);
            MemoryStream memoryStream = new(byteArray);
            return new FileStreamResult(memoryStream, "application/pdf");
        }

        public async Task<string> CreatePdfLedger(LedgerCriteria criteria, int shipOwnerId) {
            var emailQueue = new EmailQueue {
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                CustomerId = criteria.CustomerId
            };
            var linesPerPage = 55;
            var linesPrinted = 0;
            var ledger = await ProcessLedger(emailQueue, shipOwnerId);
            var locale = CultureInfo.CreateSpecificCulture("el-GR");
            GlobalFontSettings.FontResolver = new FileFontResolver();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PdfDocument document = new();
            PdfPage page = document.AddPage();
            XFont logoFont = new("ACCanterBold", 20);
            XFont robotoMonoFont = new("RobotoMono", 6);
            XFont monotypeFont = new("MonoType", 6);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.DrawString(ledger[1].ShipOwner.Description, logoFont, XBrushes.Black, new XPoint(40, 40));
            gfx.DrawString("ΚΑΡΤΕΛΑ ΠΕΛΑΤΗ: " + ledger[1].Customer.Description, robotoMonoFont, XBrushes.Black, new XPoint(40, 53));
            gfx.DrawString("ΔΙΑΣΤΗΜΑ: " + criteria.FromDate + " - " + criteria.ToDate, robotoMonoFont, XBrushes.Black, new XPoint(40, 62));
            PrintColumnHeaders(gfx, robotoMonoFont);
            int verticalPosition = 100;
            for (int i = 0; i < ledger.Count; i++) {
                verticalPosition += 12;
                linesPrinted++;
                if (linesPrinted > linesPerPage) {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    linesPrinted = 0;
                    verticalPosition = 100;
                    PrintColumnHeaders(gfx, robotoMonoFont);
                }
                gfx.DrawString(DateHelpers.FormatDateStringToLocaleString(ledger[i].Date), robotoMonoFont, XBrushes.Black, new XPoint(40, verticalPosition));
                gfx.DrawString(ledger[i].DocumentType.Description, robotoMonoFont, XBrushes.Black, new XPoint(80, verticalPosition));
                gfx.DrawString(ledger[i].DocumentType.Batch, robotoMonoFont, XBrushes.Black, new XPoint(220, verticalPosition));
                gfx.DrawString(ledger[i].InvoiceNo, robotoMonoFont, XBrushes.Black, new XPoint(270, verticalPosition));
                gfx.DrawString(ledger[i].Debit.ToString("N2", locale), monotypeFont, XBrushes.Black, new XPoint(456 - ledger[i].Debit.ToString("N2", locale).Length * 3, verticalPosition));
                gfx.DrawString(ledger[i].Credit.ToString("N2", locale), monotypeFont, XBrushes.Black, new XPoint(516 - ledger[i].Credit.ToString("N2", locale).Length * 3, verticalPosition));
                gfx.DrawString(ledger[i].Balance.ToString("N2", locale), monotypeFont, XBrushes.Black, new XPoint(576 - ledger[i].Balance.ToString("N2", locale).Length * 3, verticalPosition));
            }
            var filename = criteria.CustomerId.ToString() + "-" + shipOwnerId.ToString() + ".pdf";
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + "Ledgers" + Path.DirectorySeparatorChar + filename);
            document.Save(fullpathname);
            return filename;
        }

        private async Task<List<LedgerVM>> ProcessLedger(EmailQueue criteria, int shipOwnerId) {
            var records = ledgerSaleRepo.BuildBalanceForLedger(await ledgerSaleRepo.GetForLedger(true, criteria.FromDate, criteria.ToDate, (int)criteria.CustomerId, shipOwnerId));
            var previous = ledgerSaleRepo.BuildPrevious(records, criteria.FromDate);
            var requested = ledgerSaleRepo.BuildRequested(records, criteria.FromDate);
            var total = ledgerSaleRepo.BuildTotal(records);
            return ledgerSaleRepo.MergePreviousRequestedAndTotal(previous, requested, total);
        }

        private static void PrintColumnHeaders(XGraphics gfx, XFont robotoMonoFont) {
            gfx.DrawString("ΗΜΕΡΟΜΗΝΙΑ", robotoMonoFont, XBrushes.Black, new XPoint(40, 90));
            gfx.DrawString("ΠΑΡΑΣΤΑΤΙΚΟ", robotoMonoFont, XBrushes.Black, new XPoint(80, 90));
            gfx.DrawString("ΣΕΙΡΑ", robotoMonoFont, XBrushes.Black, new XPoint(218, 90));
            gfx.DrawString("NO", robotoMonoFont, XBrushes.Black, new XPoint(270, 90));
            gfx.DrawString("ΧΡΕΩΣΗ", robotoMonoFont, XBrushes.Black, new XPoint(434, 90));
            gfx.DrawString("ΠΙΣΤΩΣΗ", robotoMonoFont, XBrushes.Black, new XPoint(490, 90));
            gfx.DrawString("ΥΠΟΛΟΙΠΟ", robotoMonoFont, XBrushes.Black, new XPoint(547, 90));
        }

    }

}