using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Account;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.EmailServices {

    public class EmailQueueService : BackgroundService {

        #region variables

        private readonly IEmailAccountSender emailAccountSender;
        private readonly EnvironmentSettings environmentSettings;
        private readonly UserManager<UserExtended> userManager;
        private readonly IUserRepository userRepo;
        private readonly AppDbContext context;

        #endregion

        public EmailQueueService(AppDbContext context, IUserRepository userRepo, IEmailAccountSender emailAccountSender, IOptions<EnvironmentSettings> environmentSettings, UserManager<UserExtended> userManager, IServiceProvider serviceProvider) {
            this.emailAccountSender = emailAccountSender;
            this.environmentSettings = environmentSettings.Value;
            this.userManager = userManager;
            this.userRepo = userRepo;
            this.context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(value: 30), stoppingToken);
                var x = await context.EmailQueues
                    .OrderBy(x => x.Priority).ThenBy(x => x.PostAt)
                    .FirstOrDefaultAsync();
                if (x != null) {
                    if (x.Initiator == "ResetPassword") { ResetPassword(x); }
                    if (x.Initiator == "UserDetails") { }
                    if (x.Initiator == "CheckIn") { }
                    if (x.Initiator == "Sales") { }
                    if (x.Initiator == "Receipts") { }

                }

            }
        }

        private async void ResetPassword(EmailQueue z) {
            var x = userManager.Users.Where(x => x.Id == z.EntityId.ToString()).FirstOrDefaultAsync().Result;
            if (x != null) {
                string token = await userManager.GeneratePasswordResetTokenAsync(x);
                string tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                string baseUrl = environmentSettings.BaseUrl;
                string returnUrl = baseUrl + "/#/resetPassword?email=" + x.Email + "&token=" + tokenEncoded;
                var response = emailAccountSender.SendForgotPasswordEmail(x.UserName, x.Displayname, x.Email, returnUrl, "Αίτηση για αλλαγή κωδικού");
                if (response.Exception == null) {
                    context.EmailQueues.Remove(z);
                    await context.SaveChangesAsync();
                }

            }

        }

    }

}