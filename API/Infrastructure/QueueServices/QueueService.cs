using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace API.Infrastructure.QueueServices {

    public class QueueService : BackgroundService {

        public QueueService() { }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
                ProcessEmailQueue();
                ProcessReservationQueue();
            }
        }

        private static void ProcessEmailQueue() {
            Log.Information("Email Queue");
        }

        private static void ProcessReservationQueue() {
            Log.Information("Reservation Queue");
        }

    }

}