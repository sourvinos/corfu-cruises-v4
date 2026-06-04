using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using API.Features.Reservations.Customers;
using API.Features.Reservations.Destinations;
using API.Features.Reservations.Parameters;
using API.Features.Reservations.PickupPoints;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.ReservationQueueServices {

    public class ReservationQueueService : BackgroundService {

        #region variables

        private readonly EnvironmentSettings environmentSettings;
        private readonly ICustomerRepository customerRepo;
        private readonly IDestinationRepository destinationRepo;
        private readonly IMapper mapper;
        private readonly IPickupPointRepository pickupPointRepo;
        private readonly IReservationParametersRepository parametersRepo;
        private readonly IReservationQueueRepository reservationQueueRepo;
        private readonly IReservationUpdateRepository reservationUpdateRepo;
        private readonly IReservationValidation reservationValidation;
        protected readonly AppDbContext context;

        #endregion

        public ReservationQueueService(AppDbContext context, ICustomerRepository customerRepo, IDestinationRepository destinationRepo, IMapper mapper, IOptions<EnvironmentSettings> environmentSettings, IPickupPointRepository pickupPointRepo, IReservationParametersRepository parametersRepo, IReservationQueueRepository reservationQueueRepo, IReservationUpdateRepository reservationUpdateRepo, IReservationValidation reservationValidation) {
            this.context = context; this.customerRepo = customerRepo;
            this.destinationRepo = destinationRepo;
            this.environmentSettings = environmentSettings.Value;
            this.environmentSettings = environmentSettings.Value;
            this.mapper = mapper;
            this.parametersRepo = parametersRepo;
            this.parametersRepo = parametersRepo;
            this.pickupPointRepo = pickupPointRepo;
            this.reservationQueueRepo = reservationQueueRepo;
            this.reservationUpdateRepo = reservationUpdateRepo;
            this.reservationValidation = reservationValidation;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(value: environmentSettings.ReservationsSecondsDelay), stoppingToken);
                await UpdateQueue();
                await ProcessQueue();
            }
        }

        private async Task UpdateQueue() {
            if (GetParameters().LinkTwistIsActive) {
                var fromDate = DateHelpers.DateToISOString(DateHelpers.GetLocalDateTime());
                var toDate = DateHelpers.DateToISOString(DateHelpers.GetLocalDateTime().AddDays(0));
                using HttpClient httpClient = new();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
                var x = JsonSerializer.Deserialize<ReservationLinkTwist[]>(await httpClient.GetStringAsync($"{GetParameters().APIUrl}/bookings?activity_date_time_from={fromDate}&activity_date_time_to={toDate}"));
                x?.OrderBy(x => x.Details.Select(x => x.Date));
                foreach (var item in x) {
                    if (item.BookingStatus == "completed") {
                        if (reservationQueueRepo.GetByCode(item.Code).Result == null) {
                            reservationQueueRepo.Create(new ReservationQueue {
                                Code = item.Code,
                                IsImported = 0,
                                PostAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime())
                            });
                        }
                    }
                }
            }
        }

        private async Task ProcessQueue() {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
            var x = await reservationQueueRepo.GetFirstNotImported();
            if (x != null) {
                var i = JsonSerializer.Deserialize<ReservationLinkTwist>(await httpClient.GetStringAsync(GetParameters().APIUrl + "/bookings/" + x.Code));
                i.Date = DateHelpers.DateToISOString(DateHelpers.StringToDate(i.Details.FirstOrDefault().Date));
                i.Destination = GetDestination(i.Details.FirstOrDefault().Destination);
                i.OurDestination = await GetOurDestinationAsync(i.Details.FirstOrDefault().Destination);
                i.Customer = GetCustomer(i.Referer);
                i.BookingCode ??= x.Code;
                i.PickupPoint = await GetPickupPointAsync(i);
                i.Adults = i.Details.Count(x => x.Age.Contains("adult"));
                i.Kids = i.Details.Count(x => x.Age.Contains("child"));
                i.Free = i.Details.Count(x => x.Age.Contains("infant"));
                i.TotalPax = i.Adults + i.Kids + i.Free;
                i.Notes = i.Notes != null ? i.Notes.Replace("\n", "").Replace("<br/>", "").Replace("<p>", "").Replace("</p>", "") : "";
                var alreadyExists = reservationQueueRepo.GetByDateAndTicketNoAsync(i.Date, i.BookingCode).Result;
                if (alreadyExists == false) {
                    i.IsValidPrimary = ValidateReservation(i);
                    if (i.IsValidPrimary) {
                        var z = new ReservationWriteDto {
                            RefNo = AttachNewRefNoToDto(i.Destination.Id),
                            PickupPointId = i.PickupPoint.Id,
                            TicketNo = i.BookingCode,
                            CustomerId = i.Customer.Id,
                            Date = i.Date,
                            DestinationId = i.OurDestination.Id,
                            DriverId = null,
                            Email = "",
                            Adults = i.Adults,
                            Kids = i.Kids,
                            Free = i.Free,
                            LinkTwistId = x.Code,
                            Phones = "",
                            PortId = AttachPortIdToDto(i.PickupPoint.Id),
                            PortAlternateId = AttachPortIdToDto(i.PickupPoint.Id),
                            Remarks = "",
                            PostAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime()),
                            PostUser = "linktwist",
                            PutAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime()),
                            PutUser = "linktwist",
                            Notes = i.Notes ?? "",
                        };
                        var q = mapper.Map<ReservationWriteDto, Reservation>(z);
                        using var transaction = await context.Database.BeginTransactionAsync();
                        context.Add(q);
                        x.IsImported = 1;
                        await context.SaveChangesAsync();
                        transaction.Commit();
                    } else {
                        using var transaction = await context.Database.BeginTransactionAsync();
                        x.IsImported = 2;
                        await context.SaveChangesAsync();
                        transaction.Commit();
                    }
                } else {
                    using var transaction = await context.Database.BeginTransactionAsync();
                    x.IsImported = 3;
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }
            }
        }

        private string AttachNewRefNoToDto(int destinationId) {
            return reservationUpdateRepo.AssignRefNoToNewDto(destinationId);
        }

        private int AttachPortIdToDto(int pickupPointId) {
            return reservationValidation.GetPortIdFromPickupPointId(pickupPointId);
        }

        private SimpleEntity GetCustomer(string referer) {
            var x = customerRepo.GetByLinkTwistRefererAsync(referer).Result;
            return new SimpleEntity {
                Id = x != null ? x.Id : 0,
                Description = x != null ? x.Description : "",
            };
        }

        private SimpleEntity GetDestination(string destination) {
            var x = destinationRepo.GetByLinkTwistAsync(destination).Result;
            return new SimpleEntity {
                Id = x != null ? x.Id : 0,
                Description = x != null ? x.Description : "",
            };
        }

        private async Task<SimpleEntity> GetOurDestinationAsync(string destination) {
            var z = await destinationRepo.GetByLinkedIdAsync(destinationRepo.GetByLinkTwistAsync(destination).Result.LinkedId);
            return new SimpleEntity {
                Id = z != null ? z.Id : 0,
                Description = z != null ? z.Description : "",
            };
        }

        private async Task<SimpleEntity> GetPickupPointAsync(ReservationLinkTwist reservation) {
            if (reservation.Extras.Count > 0) {
                var x = await pickupPointRepo.GetByDescriptionAsync(reservation.Details[0].Port);
                return new SimpleEntity {
                    Id = x.Id,
                    Description = x.Description,
                };

            } else {
                if (reservation.Destination.Description.Contains("- no transfer", StringComparison.CurrentCultureIgnoreCase)) {
                    var x = await pickupPointRepo.GetByDescriptionAsync(reservation.Details[0].Port);
                    if (x != null) {
                        return new SimpleEntity {
                            Id = x.Id,
                            Description = x.Description,
                        };
                    }
                } else {
                    var x = pickupPointRepo.GetTempAsync("- transfer").Result;
                    if (x != null) {
                        return new SimpleEntity {
                            Id = x.Id,
                            Description = x.Description,
                        };
                    }
                }
                return new SimpleEntity {
                    Id = 0,
                    Description = "",
                };
            }
        }

        private bool ValidateReservation(ReservationLinkTwist reservation) {
            var x = reservation.Destination.Description != "" &&
                reservation.Customer.Description != "" &&
                reservation.PickupPoint.Description != "" &&
                reservation.TotalPax > 0;
            return x;
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