using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Features.CheckIn;
using API.Features.Reservations.Parameters;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Account;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
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
        private readonly IEmailAccountSender emailAccountSender;
        private readonly IEmailQueueRepository queueRepo;
        private readonly IEmailUserSender emailUserSender;
        private readonly IUserRepository userRepo;
        private readonly UserManager<UserExtended> userManager;
        private readonly IMapper mapper;
        private readonly IReservationReadRepository reservationReadRepo;
        private readonly ICheckInSendToEmail checkInSendToEmail;
        private readonly AppDbContext dbContext;

        #endregion

        public EmailQueueService(
            AppDbContext dbContext,
            ICheckInSendToEmail checkInSendToEmail,
            IEmailAccountSender emailAccountSender,
            IReservationReadRepository reservationReadRepo,
            IEmailQueueRepository queueRepo,
            IEmailUserSender emailUserSender,
            IOptions<EnvironmentSettings> environmentSettings,
            IServiceProvider serviceProvider,
            IUserRepository userRepo,
            UserManager<UserExtended> userManager,
            IMapper mapper
            ) {
            this.dbContext = dbContext;
            this.checkInSendToEmail = checkInSendToEmail;
            this.reservationReadRepo = reservationReadRepo;
            this.emailAccountSender = emailAccountSender;
            this.emailUserSender = emailUserSender;
            this.environmentSettings = environmentSettings.Value;
            this.queueRepo = queueRepo;
            this.userManager = userManager;
            this.userRepo = userRepo;
            this.mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(value: 10), stoppingToken);
                var x = await queueRepo.GetFirst();
                if (x != null) {
                    if (x.Initiator == "ResetPassword") { SendResetPassword(x); }
                    if (x.Initiator == "UserDetails") { SendUserDetails(x); }
                    if (x.Initiator == "CheckIn") { SendReservation(x); }
                    if (x.Initiator == "Sales") { }
                    if (x.Initiator == "Receipts") { }
                }

            }
        }

        private async void SendResetPassword(EmailQueue z) {
            var x = userManager.Users.Where(x => x.Id == z.EntityId.ToString()).FirstOrDefaultAsync().Result;
            if (x != null) {
                string token = await userManager.GeneratePasswordResetTokenAsync(x);
                string tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                string baseUrl = environmentSettings.BaseUrl;
                string returnUrl = baseUrl + "/#/resetPassword?email=" + x.Email + "&token=" + tokenEncoded;
                var response = emailAccountSender.SendForgotPasswordEmail(x.UserName, x.Displayname, x.Email, returnUrl, "Αίτηση για αλλαγή κωδικού");
                if (response.Exception == null) {
                    z.IsCompleted = true;
                    dbContext.SaveChanges();
                }
            }
        }

        private void SendUserDetails(EmailQueue z) {
            var x = userManager.Users.Where(x => x.Id == z.EntityId.ToString()).FirstOrDefaultAsync().Result;
            if (x != null) {
                var userDetails = new UserDetailsForEmailVM {
                    Email = x.Email,
                    Username = x.UserName,
                    Displayname = x.Displayname,
                    Url = environmentSettings.BaseUrl,
                    Subject = "Ο νέος λογαριασμός σας είναι έτοιμος",
                    CompanyPhones = ""
                };
                var response = emailUserSender.EmailUserDetails(userDetails);
                if (response.Exception == null) {
                    z.IsCompleted = true;
                    dbContext.SaveChanges();
                }
            }
        }

        private void SendReservation(EmailQueue z) {
            var x = reservationReadRepo.GetByIdAsync(z.EntityId.ToString(), true).Result;
            var i = mapper.Map<Reservation, CheckInBoardingPassReservationVM>(x);
            var response = checkInSendToEmail.SendReservationToEmail(i);
            if (response.Exception == null) {
                z.IsCompleted = true;
                dbContext.SaveChanges();
            }
        }

    }

}