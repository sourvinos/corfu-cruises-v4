using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Features.CheckIn;
using API.Features.Reservations.Reservations;
using API.Features.Sales.Invoices;
using API.Features.Sales.Ledgers;
using API.Features.Sales.Receipts;
using API.Infrastructure.Account;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using API.Infrastructure.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace API.Infrastructure.EmailServices {

    public class EmailQueueService : BackgroundService {

        #region variables

        private readonly AppDbContext appDbContext;
        private readonly EnvironmentSettings environmentSettings;
        private readonly ICheckInSendToEmail checkInSendToEmail;
        private readonly IEmailAccountSender emailAccountSender;
        private readonly IEmailQueueRepository queueRepo;
        private readonly IEmailUserDetailsSender emailUserSender;
        private readonly IInvoiceEmailSender emailInvoiceSender;
        private readonly IInvoicePdfRepository invoicePdfRepo;
        private readonly IInvoiceReadRepository invoiceReadRepo;
        private readonly IMapper mapper;
        private readonly IReceiptEmailSender emailReceiptSender;
        private readonly IReceiptPdfRepository receiptPdfRepo;
        private readonly IReceiptRepository receiptRepo;
        private readonly IReservationReadRepository reservationReadRepo;
        private readonly UserManager<UserExtended> userManager;
        private readonly ILedgerSalesRepository ledgerSaleRepo;
        private readonly ILedgerEmailSender ledgerEmailSender;

        #endregion

        public EmailQueueService(ILedgerEmailSender emailSender, AppDbContext dbContext, ICheckInSendToEmail checkInSendToEmail, IEmailAccountSender emailAccountSender, IEmailQueueRepository queueRepo, IEmailUserDetailsSender emailUserDetailsSender, IInvoiceEmailSender emailInvoiceSender, IInvoicePdfRepository invoicePdfRepo, IInvoiceReadRepository invoiceReadRepo, ILedgerSalesRepository ledgerSaleRepo, IMapper mapper, IReceiptEmailSender emailReceiptSender, IReceiptPdfRepository receiptPdfRepo, IReceiptRepository receiptRepo, IReservationReadRepository reservationReadRepo, UserManager<UserExtended> userManager, IOptions<EnvironmentSettings> environmentSettings) {
            this.appDbContext = dbContext;
            this.ledgerEmailSender = emailSender;
            this.checkInSendToEmail = checkInSendToEmail;
            this.emailAccountSender = emailAccountSender;
            this.emailInvoiceSender = emailInvoiceSender;
            this.emailReceiptSender = emailReceiptSender;
            this.emailUserSender = emailUserDetailsSender;
            this.environmentSettings = environmentSettings.Value;
            this.invoicePdfRepo = invoicePdfRepo;
            this.invoiceReadRepo = invoiceReadRepo;
            this.ledgerSaleRepo = ledgerSaleRepo;
            this.mapper = mapper;
            this.queueRepo = queueRepo;
            this.receiptPdfRepo = receiptPdfRepo;
            this.receiptRepo = receiptRepo;
            this.reservationReadRepo = reservationReadRepo;
            this.userManager = userManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(value: 10), stoppingToken);
                var x = await queueRepo.GetFirstNotCompleted();
                if (x != null) {
                    if (x.Initiator == "ResetPassword") { SendResetPassword(x); }
                    if (x.Initiator == "UserDetails") { SendUserDetails(x); }
                    if (x.Initiator == "CheckIn") { await SendReservationAsync(x); }
                    if (x.Initiator == "Sales") { await SendInvoiceAsync(x); }
                    if (x.Initiator == "Receipts") { await SendReceiptAsync(x); }
                    if (x.Initiator == "SaleLedgers") { DoStuff(x); }
                }
            }
        }

        private async void SendResetPassword(EmailQueue emailQueue) {
            var x = userManager.Users.Where(x => x.Id == emailQueue.EntityId.ToString()).FirstOrDefaultAsync().Result;
            if (x != null) {
                var response = emailAccountSender.EmailForgotPassword(x.UserName, x.Displayname, x.Email, environmentSettings.BaseUrl + "/#/resetPassword?email=" + x.Email + "&token=" + WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await userManager.GeneratePasswordResetTokenAsync(x))));
                if (response.Exception == null) {
                    emailQueue.IsCompleted = true;
                    appDbContext.SaveChanges();
                }
            }
        }

        private void SendUserDetails(EmailQueue emailQueue) {
            var x = userManager.Users.Where(x => x.Id == emailQueue.EntityId.ToString()).FirstOrDefaultAsync().Result;
            if (x != null) {
                var response = emailUserSender.EmailUserDetails(x);
                if (response.Exception == null) {
                    emailQueue.IsCompleted = true;
                    appDbContext.SaveChanges();
                }
            }
        }

        private async Task SendReservationAsync(EmailQueue emailQueue) {
            var x = await reservationReadRepo.GetByIdAsync(emailQueue.EntityId.ToString(), true);
            if (x != null) {
                var response = checkInSendToEmail.SendReservationToEmail(mapper.Map<Reservation, CheckInBoardingPassReservationVM>(x));
                if (response.Exception == null) {
                    emailQueue.IsCompleted = true;
                    appDbContext.SaveChanges();
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        private async Task SendInvoiceAsync(EmailQueue emailQueue) {
            var x = await invoiceReadRepo.GetByIdForPdfAsync(emailQueue.EntityId.ToString());
            if (x != null) {
                invoicePdfRepo.BuildPdf(mapper.Map<Invoice, InvoicePdfVM>(x));
                var response = emailInvoiceSender.SendInvoiceToEmail(emailQueue, x.Customer.Email);
                if (response.Exception == null) {
                    emailQueue.IsCompleted = true;
                    appDbContext.SaveChanges();
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        private async Task SendReceiptAsync(EmailQueue emailQueue) {
            var x = await receiptRepo.GetByIdForPdfAsync(emailQueue.EntityId.ToString());
            if (x != null) {
                receiptPdfRepo.BuildPdf(mapper.Map<Receipt, ReceiptPdfVM>(x));
                var response = emailReceiptSender.SendReceiptToEmail(emailQueue, x.Customer.Email);
                if (response.Exception == null) {
                    emailQueue.IsCompleted = true;
                    appDbContext.SaveChanges();
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        private void DoStuff(EmailQueue emailQueue) {
            var emailLedgerVM = CreatePdf(emailQueue).Result;
            var response = ledgerEmailSender.SendLedgerToEmail(emailLedgerVM);
            if (response.Exception == null) {
                emailQueue.IsCompleted = true;
                appDbContext.SaveChanges();
            }
        }

        private async Task<EmailLedgerVM> CreatePdf(EmailQueue emailQueue) {
            var childTable = await ledgerSaleRepo.GetFromChildTable(emailQueue.EntityId.ToString());
            var emailLedgerVM = new EmailLedgerVM {
                CustomerId = childTable.First().CustomerId,
                Filenames = new List<string>()
            };
            foreach (var x in childTable) {
                var z = await ledgerSaleRepo.GetForLedger(true, x.FromDate.ToString(), x.ToDate.ToString(), x.CustomerId, x.ShipOwnerId);
                if (z != null) {
                    var linesPerPage = 55;
                    var linesPrinted = 0;
                    var ledger = await ProcessLedger(x);
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
                    gfx.DrawString("ΔΙΑΣΤΗΜΑ: " + DateHelpers.FormatDateStringToLocaleString(DateHelpers.DateToISOString(x.FromDate)) + " - " + DateHelpers.FormatDateStringToLocaleString(DateHelpers.DateToISOString(x.ToDate)), robotoMonoFont, XBrushes.Black, new XPoint(40, 62));
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
                    var filename = x.CustomerId.ToString() + "-" + x.ShipOwnerId.ToString() + ".pdf";
                    var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + "Ledgers" + Path.DirectorySeparatorChar + filename);
                    document.Save(fullpathname);
                    emailLedgerVM.Filenames.Add(filename);
                }
            }
            return emailLedgerVM;
        }

        private async Task<List<LedgerVM>> ProcessLedger(EmailLedgerSaleQueue criteria) {
            var records = ledgerSaleRepo.BuildBalanceForLedger(await ledgerSaleRepo.GetForLedger(true, DateHelpers.DateToISOString(criteria.FromDate), DateHelpers.DateToISOString(criteria.ToDate), criteria.CustomerId, criteria.ShipOwnerId));
            var previous = ledgerSaleRepo.BuildPrevious(records, DateHelpers.DateToISOString(criteria.FromDate));
            var requested = ledgerSaleRepo.BuildRequested(records, DateHelpers.DateToISOString(criteria.FromDate));
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