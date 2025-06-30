using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Features.Reservations.Parameters;
using API.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.Users {

    public class SendWelcomeEmailScheduleService : BackgroundService {

        #region variables

        private readonly IEmailSender emailSender;
        private readonly EnvironmentSettings environmentSettings;
        private readonly UserManager<UserExtended> userManager;
        private readonly IReservationParametersRepository parametersRepo;
        private readonly IUserRepository userRepo;

        #endregion

        public SendWelcomeEmailScheduleService(IUserRepository userRepo, IEmailSender emailSender, IReservationParametersRepository parametersRepo, IOptions<EnvironmentSettings> environmentSettings, UserManager<UserExtended> userManager, IServiceProvider serviceProvider) {
            this.emailSender = emailSender;
            this.environmentSettings = environmentSettings.Value;
            this.userManager = userManager;
            this.parametersRepo = parametersRepo;
            this.userRepo = userRepo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(70), stoppingToken);
                var x = userManager.Users.Where(x => x.IsNewEmailPending).FirstOrDefaultAsync(cancellationToken: stoppingToken).Result;
                if (x != null) {
                    var userDetails = new UserDetailsForEmailVM {
                        Email = x.Email,
                        Username = x.UserName,
                        Displayname = x.Displayname,
                        Url = environmentSettings.BaseUrl,
                        Subject = "Ο νέος λογαριασμός σας είναι έτοιμος",
                        CompanyPhones = parametersRepo.GetAsync().Result.Phones
                    };
                    var response = emailSender.EmailUserDetails(userDetails);
                    if (response.Exception == null) {
                        await userRepo.UpdateIsNewEmailSentAsync(x);
                    }
                }
            }
        }

    }

}