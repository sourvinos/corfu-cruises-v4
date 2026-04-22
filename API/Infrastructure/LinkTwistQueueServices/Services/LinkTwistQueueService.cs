using System;
using System.Collections.Generic;
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
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using API.Infrastructure.Users;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.LinkTwistServices {

    public class LinkTwistQueueService : BackgroundService {

        #region variables

        private readonly EnvironmentSettings environmentSettings;
        private readonly ICustomerRepository customerRepo;
        private readonly IDestinationRepository destinationRepo;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILinkTwistQueueRepository linkTwistQueueRepo;
        private readonly IMapper mapper;
        private readonly IPickupPointRepository pickupPointRepo;
        private readonly IReservationParametersRepository parametersRepo;
        private readonly IReservationUpdateRepository reservationUpdateRepo;
        private readonly IReservationValidation reservationValidation;
        private readonly UserManager<UserExtended> userManager;

        #endregion

        public LinkTwistQueueService(ICustomerRepository customerRepo, IDestinationRepository destinationRepo, IHttpContextAccessor httpContextAccessor, ILinkTwistQueueRepository linkTwistQueueRepo, IMapper mapper, IOptions<EnvironmentSettings> environmentSettings, IPickupPointRepository pickupPointRepo, IReservationParametersRepository parametersRepo, IReservationUpdateRepository reservationUpdateRepo, IReservationValidation reservationValidation, UserManager<UserExtended> userManager) {
            this.customerRepo = customerRepo;
            this.destinationRepo = destinationRepo;
            this.environmentSettings = environmentSettings.Value;
            this.httpContextAccessor = httpContextAccessor;
            this.linkTwistQueueRepo = linkTwistQueueRepo;
            this.mapper = mapper;
            this.parametersRepo = parametersRepo;
            this.pickupPointRepo = pickupPointRepo;
            this.reservationUpdateRepo = reservationUpdateRepo;
            this.reservationValidation = reservationValidation;
            this.userManager = userManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Delay(TimeSpan.FromSeconds(value: environmentSettings.ReservationsSecondsDelay), stoppingToken);
                var x = await linkTwistQueueRepo.GetFirstNotCompleted();
                if (x != null) {
                    await ImportReservation(x);
                }
            }
        }

        private async Task<ResponseWithBody> ImportReservation(LinkTwistQueue linkTwistQueue) {
            var fromDate = DateHelpers.GetLocalDateTime();
            var toDate = DateHelpers.GetLocalDateTime().AddDays(10);
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
                var i = new ReservationWriteDto();
                i.RefNo = AttachNewRefNoToDto(x.Destination.Id);
                i.PickupPointId = x.PickupPoint.Id;
                i.TicketNo = x.Code;
                i.CustomerId = x.Customer.Id;
                i.Date = x.Date;
                i.DestinationId = x.OurDestination.Id;
                i.DriverId = null;
                i.Email = "x.Email";
                i.Adults = x.Adults;
                i.Kids = x.Kids;
                i.Free = x.Free;
                i.LinkTwistId = x.Code;
                i.Phones = "";
                i.PortId = AttachPortIdToDto(i.PickupPointId);
                i.PortAlternateId = AttachPortIdToDto(i.PickupPointId);
                i.Remarks = "";
                i.PostAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime());
                i.PostUser = "system";
                i.PutAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime());
                i.PutUser = "system";
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
            var x = reservationUpdateRepo.AssignRefNoToNewDto(destinationId);
            return x;
        }

        private int AttachPortIdToDto(int pickupPointId) {
            var x = reservationValidation.GetPortIdFromPickupPointId(pickupPointId);
            return x;
        }

        private ReservationParametersVM GetParameters() {
            var parameters = parametersRepo.GetAsync().Result;
            return new ReservationParametersVM {
                APIKey = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoAPIKey : parameters.LinkTwistLiveAPIKey,
                APIUrl = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoUrl : parameters.LinkTwistLiveUrl
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

        private static bool ValidatePassengers(List<LinkTwistReservationDetails> details) {
            var x = true;
            foreach (var item in details) {
                if (item.Passenger.Lastname == "" || item.Passenger.Firstname == "" || ValidateAge(item) == false || item.Passenger.Birthdate == "" || item.Passenger.Nationality == "" || item.Passenger.Gender == "") {
                    x = false;
                    break;
                }
            }
            return x;
        }

        private static bool ValidateAge(LinkTwistReservationDetails details) {
            if (details.Age.StartsWith("adult") || details.Age.StartsWith("child") || details.Age.StartsWith("infant")) {
                return true;
            } else {
                return false;
            }
        }

    }

}