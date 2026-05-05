using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using API.Features.Reservations.Customers;
using API.Features.Reservations.Destinations;
using API.Features.Reservations.LinkTwist;
using API.Features.Reservations.Parameters;
using API.Features.Reservations.PickupPoints;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.ReservationQueueServices {

    public class ReservationUpdateQueueService : BackgroundService {

        #region variables

        private readonly EnvironmentSettings environmentSettings;
        private readonly ICustomerRepository customerRepo;
        private readonly IDestinationRepository destinationRepo;
        private readonly IReservationUpdateQueueRepository linkTwistQueueRepo;
        private readonly IMapper mapper;
        private readonly IPickupPointRepository pickupPointRepo;
        private readonly IReservationParametersRepository parametersRepo;
        private readonly IReservationUpdateRepository reservationUpdateRepo;
        private readonly IReservationValidation reservationValidation;

        #endregion

        public ReservationUpdateQueueService(ICustomerRepository customerRepo, IDestinationRepository destinationRepo, IReservationUpdateQueueRepository linkTwistQueueRepo, IMapper mapper, IOptions<EnvironmentSettings> environmentSettings, IPickupPointRepository pickupPointRepo, IReservationParametersRepository parametersRepo, IReservationUpdateRepository reservationUpdateRepo, IReservationValidation reservationValidation) {
            this.customerRepo = customerRepo;
            this.destinationRepo = destinationRepo;
            this.environmentSettings = environmentSettings.Value;
            this.linkTwistQueueRepo = linkTwistQueueRepo;
            this.mapper = mapper;
            this.parametersRepo = parametersRepo;
            this.pickupPointRepo = pickupPointRepo;
            this.reservationUpdateRepo = reservationUpdateRepo;
            this.reservationValidation = reservationValidation;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(value: environmentSettings.ReservationsUpdateQueueSecondsDelay), stoppingToken);
                await UpdateLinkTwistQueue();
            }
        }

        private async Task UpdateLinkTwistQueue() {
            if (GetParameters().LinkTwistIsActive) {
                var fromDate = DateHelpers.DateToISOString(DateHelpers.GetLocalDateTime());
                var toDate = DateHelpers.DateToISOString(DateHelpers.GetLocalDateTime().AddDays(10));
                using HttpClient httpClient = new();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
                var x = JsonSerializer.Deserialize<LinkTwistReservation[]>(await httpClient.GetStringAsync($"{GetParameters().APIUrl}/bookings?activity_date_time_from={fromDate}&activity_date_time_to={toDate}"));
                x?.OrderBy(x => x.Details.Select(x => x.Date));
                foreach (var item in x) {
                    if (item.BookingStatus == "completed") {
                        if (linkTwistQueueRepo.GetByCode(item.Code).Result == null) {
                            linkTwistQueueRepo.Create(new ReservationQueue {
                                Code = item.Code,
                                IsImported = false,
                                PostAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime())
                            });
                        }
                    }
                }
            }
        }

        private async Task<ResponseWithBody> ImportReservation(ReservationQueue linkTwistQueue) {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
            var x = JsonSerializer.Deserialize<LinkTwistReservation>(await httpClient.GetStringAsync(GetParameters().APIUrl + "/bookings/" + linkTwistQueue.Code));
            x.Date = DateHelpers.DateToISOString(DateHelpers.StringToDate(x.Details.FirstOrDefault().Date));
            x.Destination = GetDestination(x.Details.FirstOrDefault().Destination);
            x.OurDestination = GetOurDestination(x.Details.FirstOrDefault().Destination);
            x.Customer = GetCustomer(x.Referer);
            x.BookingCode = x.BookingCode;
            x.PickupPoint = GetPickupPoint(x);
            x.Adults = x.Details.Count(x => x.Age.Contains("adult"));
            x.Kids = x.Details.Count(x => x.Age.Contains("child"));
            x.Free = x.Details.Count(x => x.Age.Contains("infant"));
            x.TotalPax = x.Adults + x.Kids + x.Free;
            x.Notes = x.Notes != null ? x.Notes.Replace("\n", "").Replace("<br/>", "").Replace("<p>", "").Replace("</p>", "") : "";
            x.IsValidPrimary = ValidateReservation(x);
            if (x.IsValidPrimary) {
                var i = new ReservationWriteDto {
                    RefNo = AttachNewRefNoToDto(x.Destination.Id),
                    PickupPointId = x.PickupPoint.Id,
                    TicketNo = x.Code,
                    CustomerId = x.Customer.Id,
                    Date = x.Date,
                    DestinationId = x.OurDestination.Id,
                    DriverId = null,
                    Email = "x.Email",
                    Adults = x.Adults,
                    Kids = x.Kids,
                    Free = x.Free,
                    LinkTwistId = x.Code,
                    Phones = ""
                };
                i.PortId = AttachPortIdToDto(i.PickupPointId);
                i.PortAlternateId = AttachPortIdToDto(i.PickupPointId);
                i.Remarks = "";
                i.PostAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime());
                i.PostUser = "linktwist";
                i.PutAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime());
                i.PutUser = "linktwist";
                i.Notes = x.Notes ?? "";
                reservationUpdateRepo.Create(mapper.Map<ReservationWriteDto, Reservation>(i));
                linkTwistQueue.IsImported = true;
                linkTwistQueueRepo.Update(linkTwistQueue);
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = x,
                    Message = ApiMessages.OK()
                };
            }
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Success.ToString(),
                Body = x,
                Message = ApiMessages.OK()
            };
        }

        private string AttachNewRefNoToDto(int destinationId) {
            return reservationUpdateRepo.AssignRefNoToNewDto(destinationId);
        }

        private int AttachPortIdToDto(int pickupPointId) {
            return reservationValidation.GetPortIdFromPickupPointId(pickupPointId);
        }

        private ReservationParametersVM GetParameters() {
            var parameters = parametersRepo.GetAsync().Result;
            return new ReservationParametersVM {
                APIKey = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoAPIKey : parameters.LinkTwistLiveAPIKey,
                APIUrl = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoUrl : parameters.LinkTwistLiveUrl,
                LinkTwistIsActive = parameters.LinkTwistIsActive
            };
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

        private SimpleEntity GetOurDestination(string destination) {
            var z = destinationRepo.GetByLinkedIdAsync(destinationRepo.GetByLinkTwistAsync(destination).Result.LinkedId).Result;
            return new SimpleEntity {
                Id = z != null ? z.Id : 0,
                Description = z != null ? z.Description : "",
            };
        }

        private SimpleEntity GetPickupPoint(LinkTwistReservation reservation) {
            if (reservation.Extras.Count > 0) {
                var x = pickupPointRepo.GetByDescriptionAsync(reservation.Details[0].Port).Result;
                return new SimpleEntity {
                    Id = x.Id,
                    Description = x.Description,
                };

            } else {
                if (reservation.Destination.Description.Contains("- no transfer", StringComparison.CurrentCultureIgnoreCase)) {
                    var x = pickupPointRepo.GetByDescriptionAsync(reservation.Details[0].Port).Result;
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
                    Id = 9999,
                    Description = "(ERROR)",
                };
            }
        }

        private static bool ValidateReservation(LinkTwistReservation reservation) {
            if (reservation.Destination.Description != "" && reservation.Customer.Description != "" && reservation.PickupPoint.Description != "" && reservation.TotalPax > 0) {
                return true;
            } else {
                return false;
            }
        }

    }

}