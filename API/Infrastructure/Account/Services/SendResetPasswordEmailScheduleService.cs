using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Helpers;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.Account {

    public class SendResetPasswordEmailScheduleService : BackgroundService {

        #region variables

        private readonly IEmailSender emailSender;
        private readonly EnvironmentSettings environmentSettings;
        private readonly UserManager<UserExtended> userManager;
        private readonly IUserRepository userRepo;

        #endregion

        public SendResetPasswordEmailScheduleService(IUserRepository userRepo, IEmailSender emailSender, IOptions<EnvironmentSettings> environmentSettings, UserManager<UserExtended> userManager, IServiceProvider serviceProvider) {
            this.emailSender = emailSender;
            this.environmentSettings = environmentSettings.Value;
            this.userManager = userManager;
            this.userRepo = userRepo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(110), stoppingToken);
                var x = userManager.Users.Where(x => x.IsResetPasswordEmailPending).FirstOrDefaultAsync(cancellationToken: stoppingToken).Result;
                if (x != null) {
                    string token = await userManager.GeneratePasswordResetTokenAsync(x);
                    string tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                    string baseUrl = environmentSettings.BaseUrl;
                    string returnUrl = baseUrl + "/#/resetPassword?email=" + x.Email + "&token=" + tokenEncoded;
                    // var response = emailSender.SendForgotPasswordEmail(x.UserName, x.Displayname, x.Email, returnUrl, "Αίτηση για αλλαγή κωδικού");
                    // if (response.Exception == null) {
                    //     await userRepo.UpdateIsResetPasswordEmailSentAsync(x);
                    // }
                }
            }
        }

    }

}

