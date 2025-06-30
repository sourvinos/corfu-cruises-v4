using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace API.Features.Sales.Ledgers {

    [Route("api/[controller]")]
    public class LedgersSalesController : ControllerBase {

        #region variables

        private readonly ILedgerEmailSender emailSender;
        private readonly ILedgerSalesRepository repo;

        #endregion

        public LedgersSalesController(ILedgerEmailSender emailSender, ILedgerSalesRepository repo) {
            this.emailSender = emailSender;
            this.repo = repo;
        }

        [HttpPost("buildLedger")]
        [Authorize(Roles = "admin")]
        public Task<List<LedgerVM>> BuildLedger([FromBody] LedgerCriteria criteria) {
            return ProcessLedger(criteria);
        }

        [HttpPost("buildLedgerPdf")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> BuildLedgerPdf([FromBody] LedgerCriteria criteria) {
            var linesPerPage = 55;
            var linesPrinted = 0;
            var ledger = await ProcessLedger(criteria);
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
            gfx.DrawString("ΔΙΑΣΤΗΜΑ: " + DateHelpers.FormatDateStringToLocaleString(criteria.FromDate) + " - " + DateHelpers.FormatDateStringToLocaleString(criteria.ToDate), robotoMonoFont, XBrushes.Black, new XPoint(40, 62));
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
            var filename = criteria.CustomerId.ToString() + "-" + criteria.ShipOwnerId.ToString() + ".pdf";
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + "Ledgers" + Path.DirectorySeparatorChar + filename);
            document.Save(fullpathname);
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK(),
                Body = filename
            };
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "admin")]
        public Response EmailLedger([FromBody] EmailLedgerVM model) {
            var response = emailSender.SendLedgerToEmail(model);
            if (response.Exception == null) {
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                return new Response {
                    Code = 498,
                    Icon = Icons.Error.ToString(),
                    Id = null,
                    Message = response.Exception.Message
                };
            }
        }

        [HttpGet("[action]/{filename}")]
        [Authorize(Roles = "admin")]
        public IActionResult OpenPdf([FromRoute] string filename) {
            return emailSender.OpenPdf(filename);
        }

        private async Task<List<LedgerVM>> ProcessLedger(LedgerCriteria criteria) {
            var records = repo.BuildBalanceForLedger(await repo.GetForLedger(criteria.FromDate, criteria.ToDate, criteria.CustomerId, criteria.ShipOwnerId));
            var previous = repo.BuildPrevious(records, criteria.FromDate);
            var requested = repo.BuildRequested(records, criteria.FromDate);
            var total = repo.BuildTotal(records);
            return repo.MergePreviousRequestedAndTotal(previous, requested, total);
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