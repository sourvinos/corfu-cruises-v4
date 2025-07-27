using System;
using System.Collections.Generic;
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
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using API.Infrastructure.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.EmailServices {

    public class EmailQueueService : BackgroundService {

        #region variables

        private readonly EnvironmentSettings environmentSettings;
        private readonly ICheckInSendToEmail checkInSendToEmail;
        private readonly IEmailAccountSender emailAccountSender;
        private readonly IEmailInvoiceSender emailInvoiceSender;
        private readonly IEmailQueueRepository emailQueueRepo;
        private readonly IEmailReceiptSender emailReceiptSender;
        private readonly IEmailUserDetailsSender emailUserSender;
        private readonly IInvoicePdfRepository invoicePdfRepo;
        private readonly IInvoiceReadRepository invoiceReadRepo;
        private readonly IInvoiceUpdateRepository invoiceUpdateRepo;
        private readonly ILedgerEmailSender ledgerEmailSender;
        private readonly ILedgerPdfBuilder ledgerPdfBuilder;
        private readonly IMapper mapper;
        private readonly IReceiptPdfRepository receiptPdfRepo;
        private readonly IReceiptRepository receiptRepo;
        private readonly IReservationReadRepository reservationReadRepo;
        private readonly UserManager<UserExtended> userManager;

        #endregion

        public EmailQueueService(ICheckInSendToEmail checkInSendToEmail, IEmailAccountSender emailAccountSender, IEmailInvoiceSender emailInvoiceSender, IEmailQueueRepository queueRepo, IEmailReceiptSender emailReceiptSender, IEmailUserDetailsSender emailUserDetailsSender, IInvoicePdfRepository invoicePdfRepo, IInvoiceReadRepository invoiceReadRepo, IInvoiceUpdateRepository invoiceUpdateRepo, ILedgerEmailSender emailSender, ILedgerPdfBuilder ledgerPdfBuilder, IMapper mapper, IOptions<EnvironmentSettings> environmentSettings, IReceiptPdfRepository receiptPdfRepo, IReceiptRepository receiptRepo, IReservationReadRepository reservationReadRepo, UserManager<UserExtended> userManager) {
            this.checkInSendToEmail = checkInSendToEmail;
            this.emailAccountSender = emailAccountSender;
            this.emailInvoiceSender = emailInvoiceSender;
            this.emailQueueRepo = queueRepo;
            this.emailReceiptSender = emailReceiptSender;
            this.emailUserSender = emailUserDetailsSender;
            this.environmentSettings = environmentSettings.Value;
            this.invoicePdfRepo = invoicePdfRepo;
            this.invoiceReadRepo = invoiceReadRepo;
            this.invoiceUpdateRepo = invoiceUpdateRepo;
            this.ledgerEmailSender = emailSender;
            this.ledgerPdfBuilder = ledgerPdfBuilder;
            this.mapper = mapper;
            this.receiptPdfRepo = receiptPdfRepo;
            this.receiptRepo = receiptRepo;
            this.reservationReadRepo = reservationReadRepo;
            this.userManager = userManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(value: environmentSettings.EmailSecondsDelay), stoppingToken);
                var x = await emailQueueRepo.GetFirstNotCompleted();
                if (x != null) {
                    if (x.Initiator == "ResetPassword") { SendResetPassword(x); }
                    if (x.Initiator == "UserDetails") { await SendUserDetailsAsync(x); }
                    if (x.Initiator == "CheckIn") { await SendReservationAsync(x); }
                    if (x.Initiator == "Invoices") { await DoInvoiceTasks(x); }
                    if (x.Initiator == "Receipts") { await DoReceiptTasks(x); }
                    if (x.Initiator == "SaleLedgers") { await SendLedgerAsync(x); }
                }
            }
        }

        private async void SendResetPassword(EmailQueue emailQueue) {
            var x = userManager.Users.Where(x => x.Id == emailQueue.EntityId.ToString()).FirstOrDefaultAsync().Result;
            if (x != null) {
                var response = emailAccountSender.EmailForgotPassword(x.UserName, x.Displayname, x.Email, environmentSettings.BaseUrl + "/#/resetPassword?email=" + x.Email + "&token=" + WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await userManager.GeneratePasswordResetTokenAsync(x))));
                if (response.Exception == null) {
                    emailQueue.IsSent = true;
                    emailQueueRepo.Update(emailQueue);
                }
            }
        }

        private async Task SendUserDetailsAsync(EmailQueue emailQueue) {
            var x = await userManager.Users.Where(x => x.Id == emailQueue.EntityId.ToString()).FirstOrDefaultAsync();
            if (x != null) {
                var response = emailUserSender.EmailUserDetails(x);
                if (response.Exception == null) {
                    emailQueue.IsSent = true;
                    emailQueueRepo.Update(emailQueue);
                }
            }
        }

        private async Task SendReservationAsync(EmailQueue emailQueue) {
            var x = await reservationReadRepo.GetByIdAsync(emailQueue.EntityId.ToString(), true);
            if (x != null) {
                var response = checkInSendToEmail.SendReservationToEmail(mapper.Map<Reservation, CheckInBoardingPassReservationVM>(x));
                if (response.Exception == null) {
                    emailQueue.IsSent = true;
                    emailQueueRepo.Update(emailQueue);
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        private async Task DoInvoiceTasks(EmailQueue emailQueue) {
            var invoice = await invoiceReadRepo.GetByIdForPdfAsync(emailQueue.EntityId.ToString());
            if (invoice != null) {
                if (invoicePdfRepo.BuildPdf(mapper.Map<Invoice, InvoicePdfVM>(invoice)) != "") {
                    if (emailInvoiceSender.SendInvoiceToEmail(emailQueue, invoice.Customer.Email).Exception == null) {
                        emailQueue.IsSent = true;
                        emailQueueRepo.Update(emailQueue);
                        invoice.IsEmailPending = false;
                        invoice.IsEmailSent = true;
                        invoiceUpdateRepo.Update(invoice);
                    }
                }
            }
        }

        private async Task DoReceiptTasks(EmailQueue emailQueue) {
            var receipt = await receiptRepo.GetByIdForPdfAsync(emailQueue.EntityId.ToString());
            if (receipt != null) {
                if (receiptPdfRepo.BuildPdf(mapper.Map<Receipt, ReceiptPdfVM>(receipt)) != "") {
                    if (emailReceiptSender.SendReceiptToEmail(emailQueue, receipt.Customer.Email).Exception == null) {
                        emailQueue.IsSent = true;
                        emailQueueRepo.Update(emailQueue);
                        receipt.IsEmailPending = false;
                        receipt.IsEmailSent = true;
                        receiptRepo.Update(receipt);
                    }
                }
            }
        }

        private async Task SendLedgerAsync(EmailQueue emailQueue) {
            var ledgerCriteria = new LedgerCriteria {
                FromDate = emailQueue.FromDate,
                ToDate = emailQueue.ToDate,
                CustomerId = (int)emailQueue.CustomerId,
            };
            var filenames = new List<string>();
            for (int i = 1; i <= 2; i++) {
                filenames.Add(await ledgerPdfBuilder.CreatePdfLedger(ledgerCriteria, i));
            }
            var x = new EmailLedgerVM {
                CustomerId = (int)emailQueue.CustomerId,
                Filenames = filenames
            };
            var response = ledgerEmailSender.SendLedgerToEmail(x);
            if (response.Exception == null) {
                emailQueue.IsSent = true;
                emailQueueRepo.Update(emailQueue);
            }
        }

    }

}