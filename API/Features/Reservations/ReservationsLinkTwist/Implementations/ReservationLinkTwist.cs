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

namespace API.Features.Reservations.LinkTwist {

    public class ReservationLinkTwist : Repository<ReservationLinkTwist>, IReservationLinkTwist {

        #region variables

        private readonly ICustomerRepository customerRepo;
        private readonly IDestinationRepository destinationRepo;
        private readonly IPickupPointRepository pickupPointRepo;
        private readonly IReservationParametersRepository parametersRepo;

        #endregion

        public ReservationLinkTwist(AppDbContext appDbContext, ICustomerRepository customerRepo, IDestinationRepository destinationRepo, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, IPickupPointRepository pickupPointRepo, IReservationParametersRepository parametersRepo, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.customerRepo = customerRepo;
            this.destinationRepo = destinationRepo;
            this.parametersRepo = parametersRepo;
            this.pickupPointRepo = pickupPointRepo;
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
            x.Customer = GetCustomer(x.Referer);
            x.PickupPoint = GetPickupPoint(x);
            x.Adults = x.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Adult"));
            x.Kids = x.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Child"));
            x.Free = x.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Infant"));
            x.Date = DateHelpers.DateToISOString(DateHelpers.StringToDate(x.Details.FirstOrDefault().Date));
            x.TotalPax = x.Adults + x.Kids + x.Free;
            x.Status = GetStatus(x.BookingStatus);
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
                item.PickupPoint = GetPickupPoint(item);
                item.Adults = item.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Adult"));
                item.Kids = item.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Child"));
                item.Free = item.Details.Select(x => x.Passenger).Count(x => x.Age.Contains("Infant"));
                item.TotalPax = item.Adults + item.Kids + item.Free;
                item.Status = GetStatus(item.BookingStatus);
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

        private SimpleEntity GetPickupPoint(LinkTwistReservation reservation) {
            if (reservation.Extras.Count > 0) {
                var x = pickupPointRepo.GetByLinkTwistAsync(reservation.Extras[0].Description).Result;
                return new SimpleEntity {
                    Id = x != null ? x.Id : 0,
                    Description = x != null ? x.Description : "",
                };
            } else {
                return new SimpleEntity {
                    Id = 0,
                    Description = "",
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

    }

}