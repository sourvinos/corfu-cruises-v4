using System.Threading.Tasks;
using API.Features.Reservations.Parameters;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Linq;
using API.Infrastructure.Helpers;
using System;
using API.Features.Reservations.Destinations;
using API.Infrastructure.Classes;
using API.Features.Reservations.Customers;
using API.Features.Reservations.PickupPoints;
using System.Collections.Generic;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using API.Infrastructure.Users;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Features.Reservations.Reservations;
using API.Features.Reservations.Ports;

namespace API.Features.Reservations.LinkTwist {

    public class ReservationLinkTwist : Repository<ReservationLinkTwist>, IReservationLinkTwist {

        #region variables

        private readonly ICustomerRepository customerRepo;
        private readonly IDestinationRepository destinationRepo;
        private readonly IPickupPointRepository pickupPointRepo;
        private readonly IPortRepository portRepo;
        private readonly IReservationParametersRepository parametersRepo;

        #endregion

        public ReservationLinkTwist(AppDbContext appDbContext, ICustomerRepository customerRepo, IDestinationRepository destinationRepo, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, IPickupPointRepository pickupPointRepo, IPortRepository portRepo, IReservationParametersRepository parametersRepo, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.customerRepo = customerRepo;
            this.destinationRepo = destinationRepo;
            this.parametersRepo = parametersRepo;
            this.pickupPointRepo = pickupPointRepo;
            this.portRepo = portRepo;
        }

        public async Task<IEnumerable<LinkTwistStatus>> GetAsync() {
            List<LinkTwistStatus> statuses = await context.LinkTwistStatuses
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return statuses;
        }

        public async Task<LinkTwistReservation> GetReservationAsync(string code) {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
            var x = JsonSerializer.Deserialize<LinkTwistReservation>(await httpClient.GetStringAsync(GetParameters().APIUrl + "/bookings/" + code));
            x.Date = DateHelpers.DateToISOString(DateHelpers.StringToDate(x.Details.FirstOrDefault().Date));
            x.Destination = GetDestination(x.Details.FirstOrDefault().Destination);
            x.Port = GetPort(x.Details.FirstOrDefault().Port);
            x.Customer = GetCustomer(x.Referer);
            x.BookingCode = x.BookingCode;
            x.PickupPoint = GetPickupPoint(x);
            x.Adults = x.Details.Count(x => x.Age.Contains("adult"));
            x.Kids = x.Details.Count(x => x.Age.Contains("child"));
            x.Free = x.Details.Count(x => x.Age.Contains("infant"));
            x.Date = DateHelpers.DateToISOString(DateHelpers.StringToDate(x.Details.FirstOrDefault().Date));
            x.TotalPax = x.Adults + x.Kids + x.Free;
            x.Comments = x.Comments != null ? x.Comments.Replace("\n", "").Replace("<br/>", "").Replace("<p>", "").Replace("</p>", "") : "";
            x.Status = GetStatus(x.BookingStatus);
            x.IsValidPrimary = ValidateReservation(x);
            x.IsValidSecondary = ValidatePassengers(x.Details);
            return x;
        }

        public async Task<LinkTwistReservation[]> GetReservationsAsync(LinkTwistReservationCriteriaVM criteria) {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("API-Key", GetParameters().APIKey);
            var x = JsonSerializer.Deserialize<LinkTwistReservation[]>(await httpClient.GetStringAsync($"{GetParameters().APIUrl}/bookings?activity_date_time_from={criteria.FromDate}&activity_date_time_to={criteria.ToDate}"));
            foreach (var item in x) {
                item.Date = DateHelpers.DateToISOString(DateHelpers.StringToDate(item.Details.FirstOrDefault().Date));
                item.Destination = GetDestination(item.Details.FirstOrDefault().Destination);
                item.Customer = GetCustomer(item.Referer);
                item.BookingCode = item.BookingCode;
                item.PickupPoint = GetPickupPoint(item);
                item.Adults = item.Details.Count(x => x.Age.Contains("adult"));
                item.Kids = item.Details.Count(x => x.Age.Contains("child"));
                item.Free = item.Details.Count(x => x.Age.Contains("infant"));
                item.TotalPax = item.Adults + item.Kids + item.Free;
                item.Comments = item.Comments != null ? item.Comments.Replace("\n", "").Replace("<br/>", "").Replace("<p>", "").Replace("</p>", "") : "";
                item.Status = GetStatus(item.BookingStatus);
                item.IsValidPrimary = ValidateReservation(item);
                item.IsValidSecondary = ValidatePassengers(item.Details);
            }
            return x;
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

        private SimpleEntity GetPort(string port) {
            var x = portRepo.GetByLinkTwistAsync(port).Result;
            return new SimpleEntity {
                Id = x != null ? x.Id : 0,
                Description = x != null ? x.Description : "",
            };
        }

        private SimpleEntity GetPickupPoint(LinkTwistReservation reservation) {
            if (reservation.Extras.Count > 0) {
                var x = pickupPointRepo.GetByLinkTwistAsync(reservation.Extras[0].Description).Result;
                var z = pickupPointRepo.GetTempAsync("- transfer").Result;
                return new SimpleEntity {
                    Id = x != null ? x.Id : z != null ? z.Id : 9999,
                    Description = x != null ? x.Description : z != null ? z.Description : "(ERROR)",
                };
            } else {
                if (reservation.Destination.Description.Contains("- no transfer", StringComparison.CurrentCultureIgnoreCase)) {
                    var x = pickupPointRepo.GetTempAsync("- no transfer").Result;
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

        private SimpleEntity GetStatus(string status) {
            var x = context.LinkTwistStatuses.Where(x => x.LinkTwistDescription == status).SingleOrDefaultAsync().Result;
            return new SimpleEntity {
                Id = x != null ? x.Id : 0,
                Description = x != null ? x.Description : "",
            };
        }

        private ReservationParametersVM GetParameters() {
            var parameters = parametersRepo.GetAsync().Result;
            return new ReservationParametersVM {
                APIKey = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoAPIKey : parameters.LinkTwistLiveAPIKey,
                APIUrl = parameters.LinkTwistIsDemo ? parameters.LinkTwistDemoUrl : parameters.LinkTwistLiveUrl
            };
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