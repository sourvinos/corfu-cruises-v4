using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using API.Features.Reservations.Parameters;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.ReservationQueueServices {

    public class ReservationUpdateQueueService : BackgroundService {

        #region variables

        private readonly EnvironmentSettings environmentSettings;
        private readonly IReservationParametersRepository parametersRepo;
        private readonly IReservationUpdateQueueRepository linkTwistQueueRepo;

        #endregion

        public ReservationUpdateQueueService(IReservationUpdateQueueRepository linkTwistQueueRepo, IOptions<EnvironmentSettings> environmentSettings, IReservationParametersRepository parametersRepo) {
            this.environmentSettings = environmentSettings.Value;
            this.linkTwistQueueRepo = linkTwistQueueRepo;
            this.parametersRepo = parametersRepo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(value: environmentSettings.ReservationsUpdateQueueSecondsDelay), stoppingToken);
                await UpdateQueue();
            }
        }

        private async Task UpdateQueue() {
            if (GetParameters().LinkTwistIsActive) {
                var fromDate = DateHelpers.DateToISOString(DateHelpers.GetLocalDateTime());
                var toDate = DateHelpers.DateToISOString(DateHelpers.GetLocalDateTime().AddDays(2));
                using HttpClient httpClient = new();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
                var x = JsonSerializer.Deserialize<ReservationLinkTwist[]>(await httpClient.GetStringAsync($"{GetParameters().APIUrl}/bookings?activity_date_time_from={fromDate}&activity_date_time_to={toDate}"));
                x?.OrderBy(x => x.Details.Select(x => x.Date));
                foreach (var item in x) {
                    if (item.BookingStatus == "completed") {
                        if (linkTwistQueueRepo.GetByCode(item.Code).Result == null) {
                            linkTwistQueueRepo.Create(new ReservationQueue {
                                Code = item.Code,
                                IsImported = 0,
                                PostAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime())
                            });
                        }
                    }
                }
            }
        }

        private ReservationParametersVM GetParameters() {
            var parameters = parametersRepo.GetAsync().Result;
            return new ReservationParametersVM {
                APIKey = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoAPIKey : parameters.LinkTwistLiveAPIKey,
                APIUrl = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoUrl : parameters.LinkTwistLiveUrl,
                LinkTwistIsActive = parameters.LinkTwistIsActive
            };
        }

    }

}