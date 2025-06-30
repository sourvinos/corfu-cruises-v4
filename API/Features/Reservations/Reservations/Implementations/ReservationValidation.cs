using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Features.Reservations.PickupPoints;
using API.Features.Reservations.Schedules;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Features.Reservations.Availability;
using API.Features.Reservations.Customers;
using API.Infrastructure.Responses;

namespace API.Features.Reservations.Reservations {

    public class ReservationValidation : Repository<Reservation>, IReservationValidation {

        private readonly IAvailabilityCalendar availabilityCalendar;
        private readonly ICustomerRepository customerRepo;
        private readonly IHttpContextAccessor httpContext;
        private readonly IReservationCalculatePaxLimit reservationCalculatePaxLimit;
        private readonly TestingEnvironment testingEnvironment;
        private readonly UserManager<UserExtended> userManager;

        public ReservationValidation(AppDbContext context, IAvailabilityCalendar availabilityCalendar, ICustomerRepository customerRepo, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, IReservationCalculatePaxLimit reservationCalculatePaxLimit, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.availabilityCalendar = availabilityCalendar;
            this.customerRepo = customerRepo;
            this.httpContext = httpContext;
            this.reservationCalculatePaxLimit = reservationCalculatePaxLimit;
            this.testingEnvironment = testingEnvironment.Value;
            this.userManager = userManager;
        }

        public bool IsUserOwner(int customerId) {
            var userId = Identity.GetConnectedUserId(httpContext);
            var userDetails = Identity.GetConnectedUserDetails(userManager, userId);
            return userDetails.CustomerId == customerId;
        }

        public async Task<bool> IsKeyUnique(ReservationWriteDto reservation) {
            return !await context.Reservations
                .AsNoTracking()
                .AnyAsync(x =>
                    x.Date == Convert.ToDateTime(reservation.Date) &&
                    x.ReservationId != reservation.ReservationId &&
                    x.DestinationId == reservation.DestinationId &&
                    x.CustomerId == reservation.CustomerId &&
                    string.Equals(x.TicketNo, reservation.TicketNo, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsAlreadyUpdated(Reservation z, ReservationWriteDto reservation) {
            return z != null && z.PutAt != reservation.PutAt;
        }

        public async Task<bool> IsRefNoUnique(ReservationWriteDto reservation) {
            return !await context.Reservations
                .AsNoTracking()
                .AnyAsync(x =>
                    x.RefNo == reservation.RefNo &&
                    x.ReservationId != reservation.ReservationId);
        }

        public int GetPortIdFromPickupPointId(ReservationWriteDto reservation) {
            PickupPoint pickupPoint = context.PickupPoints
                .AsNoTracking()
                .SingleOrDefault(x => x.Id == reservation.PickupPointId);
            return pickupPoint != null ? pickupPoint.PortId : 0;
        }

        public int OverbookedPax(string date, int destinationId) {
            int maxPassengersForAllPorts = context.Schedules
                .AsNoTracking()
                .Where(x => x.Date == Convert.ToDateTime(date) && x.DestinationId == destinationId)
                .Sum(x => x.MaxPax);
            int totalPaxFromAllPorts = context.Reservations
                .AsNoTracking()
                .Where(x => x.Date == Convert.ToDateTime(date) && x.DestinationId == destinationId)
                .Sum(x => x.TotalPax);
            return maxPassengersForAllPorts - totalPaxFromAllPorts;
        }

        public async Task<int> IsValidAsync(Reservation z, ReservationWriteDto reservation, IScheduleRepository scheduleRepo) {
            return true switch {
                var x when x == !await IsKeyUnique(reservation) => 409,
                var x when x == !await IsRefNoUnique(reservation) => 414,
                var x when x == !await IsValidCustomer(reservation) => 450,
                var x when x == !await IsValidDestination(reservation) => 451,
                var x when x == !await IsValidDriver(reservation) => 453,
                var x when x == !await IsValidPickupPoint(reservation) => 452,
                var x when x == !await IsValidPortAlternate(reservation) => 460,
                var x when x == !await IsValidShip(reservation) => 454,
                var x when x == !await IsValidNationality(reservation) => 456,
                var x when x == !await IsValidGenderAsync(reservation) => 457,
                var x when x == !IsCorrectPassengerCount(reservation) => 455,
                var x when x == !await PortHasDepartureForDateAndDestinationAsync(reservation) => 410,
                var x when x == !SimpleUserHasGivenCorrectCustomerId(reservation) => 413,
                var x when x == await IsSimpleUserCausingOverbooking(reservation) => 433,
                var x when x == await SimpleUserHasNightRestrictions(reservation) => 459,
                var x when x == await IsPaxLimitForSimpleUserExceededAsync(reservation) => 469,
                var x when x == SimpleUserCanNotAddReservationAfterDeparture(reservation) => 431,
                var x when x == IsAlreadyUpdated(z, reservation) => 415,
                _ => 200,
            };
        }

        private async Task<bool> PortHasDepartureForDateAndDestinationAsync(ReservationWriteDto reservation) {
            var schedule = await context.Schedules
                .AsNoTracking()
                .Where(x => x.Date.ToString() == reservation.Date && x.DestinationId == reservation.DestinationId && x.PortId == GetPortIdFromPickupPointId(reservation) && x.IsActive)
                .ToListAsync();
            return schedule.Count != 0;
        }

        private async Task<bool> IsSimpleUserCausingOverbooking(ReservationWriteDto reservation) {
            if (Identity.IsUserAdmin(httpContext) || reservation.ReservationId != Guid.Empty) {
                return false;
            } else {
                return !IsFreePaxGreaterThanZero(reservation, availabilityCalendar.CalculateFreePax(availabilityCalendar.GetPaxPerPort(availabilityCalendar.AddBatchId(await availabilityCalendar.GetScheduleAsync(reservation.Date, reservation.Date)), await availabilityCalendar.GetReservationsAsync(reservation.Date, reservation.Date))));
            }
        }

        private bool SimpleUserCanNotAddReservationAfterDeparture(ReservationWriteDto reservation) {
            return !Identity.IsUserAdmin(httpContext) && IsAfterDeparture(reservation).Result;
        }

        private async Task<bool> IsValidCustomer(ReservationWriteDto reservation) {
            if (reservation.ReservationId == Guid.Empty) {
                return await context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == reservation.CustomerId && x.IsActive) != null;
            }
            return await context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == reservation.CustomerId) != null;
        }

        private async Task<bool> IsValidDestination(ReservationWriteDto reservation) {
            if (reservation.ReservationId == Guid.Empty) {
                return await context.Destinations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == reservation.DestinationId && x.IsActive) != null;
            }
            return await context.Destinations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == reservation.DestinationId) != null;
        }

        private async Task<bool> IsValidPickupPoint(ReservationWriteDto reservation) {
            if (reservation.ReservationId == Guid.Empty) {
                return await context.PickupPoints
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == reservation.PickupPointId && x.IsActive) != null;
            }
            return await context.PickupPoints
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == reservation.PickupPointId) != null;
        }

        private async Task<bool> IsValidPortAlternate(ReservationWriteDto reservation) {
            if (reservation.PortAlternateId != null && reservation.PortAlternateId != 0) {
                if (reservation.ReservationId == Guid.Empty) {
                    var port = await context.Ports
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == reservation.PortAlternateId && x.IsActive);
                    if (port == null)
                        return false;
                } else {
                    var port = await context.Ports
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == reservation.PortAlternateId);
                    if (port == null)
                        return false;
                }
                return await context.Ports
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == reservation.PortAlternateId) != null;
            }
            return true;
        }

        private async Task<bool> IsValidDriver(ReservationWriteDto reservation) {
            if (reservation.DriverId != null && reservation.DriverId != 0) {
                if (reservation.ReservationId == Guid.Empty) {
                    var driver = await context.Drivers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == reservation.DriverId && x.IsActive);
                    if (driver == null)
                        return false;
                } else {
                    var driver = await context.Drivers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == reservation.DriverId);
                    if (driver == null)
                        return false;
                }
            }
            return true;
        }

        private async Task<bool> IsValidShip(ReservationWriteDto reservation) {
            if (reservation.ShipId != null && reservation.ShipId != 0) {
                if (reservation.ReservationId == Guid.Empty) {
                    var ship = await context.Ships
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == reservation.ShipId && x.IsActive);
                    if (ship == null)
                        return false;
                } else {
                    var ship = await context.Ships
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == reservation.ShipId);
                    if (ship == null)
                        return false;
                }
            }
            return true;
        }

        private static bool IsCorrectPassengerCount(ReservationWriteDto reservation) {
            if (reservation.Passengers != null) {
                if (reservation.Passengers.Count != 0) {
                    return reservation.Passengers.Count <= reservation.Adults + reservation.Kids + reservation.Free;
                }
            }
            return true;
        }

        private async Task<bool> IsValidNationality(ReservationWriteDto reservation) {
            if (reservation.Passengers != null) {
                bool isValid = false;
                foreach (var passenger in reservation.Passengers) {
                    if (reservation.ReservationId == Guid.Empty) {
                        isValid = await context.Nationalities
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == passenger.NationalityId && x.IsActive) != null;
                    } else {
                        isValid = await context.Nationalities
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == passenger.NationalityId) != null;
                    }
                }
                return reservation.Passengers.Count == 0 || isValid;
            }
            return true;
        }

        private async Task<bool> IsValidGenderAsync(ReservationWriteDto reservation) {
            if (reservation.Passengers != null) {
                bool isValid = false;
                foreach (var passenger in reservation.Passengers) {
                    if (reservation.ReservationId == Guid.Empty) {
                        isValid = await context.Genders
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == passenger.GenderId && x.IsActive) != null;
                    } else {
                        isValid = await context.Genders
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == passenger.GenderId) != null;
                    }
                }
                return reservation.Passengers.Count == 0 || isValid;
            }
            return true;
        }

        private bool SimpleUserHasGivenCorrectCustomerId(ReservationWriteDto reservation) {
            if (!Identity.IsUserAdmin(httpContext)) {
                var simpleUser = Identity.GetConnectedUserId(httpContext);
                var connectedUserDetails = Identity.GetConnectedUserDetails(userManager, simpleUser);
                if (connectedUserDetails.CustomerId == reservation.CustomerId) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }

        private async Task<bool> SimpleUserHasNightRestrictions(ReservationWriteDto reservation) {
            if (!Identity.IsUserAdmin(httpContext) && reservation.ReservationId == Guid.Empty) {
                if (await HasTransferAsync(reservation.PickupPointId)) {
                    if (IsForTomorrow(reservation)) {
                        if (IsBetweenClosingTimeAndMidnight(reservation)) {
                            return true;
                        }
                    }
                    if (IsForToday(reservation)) {
                        if (await IsBetweenMidnightAndDeparture(reservation)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private async Task<bool> HasTransferAsync(int pickupPointId) {
            var pickupPoint = await context.PickupPoints
                .AsNoTracking()
                .Include(x => x.CoachRoute)
                .FirstOrDefaultAsync(x => x.Id == pickupPointId);
            return pickupPoint.CoachRoute.HasTransfer;
        }

        private bool IsForTomorrow(ReservationWriteDto reservation) {
            var tomorrow = testingEnvironment.IsTesting ? reservation.Now.AddDays(1) : DateHelpers.GetLocalDateTime().AddDays(1);
            var tomorrowDate = DateHelpers.DateToISOString(tomorrow);
            return reservation.Date == tomorrowDate;
        }

        private bool IsForToday(ReservationWriteDto reservation) {
            var today = testingEnvironment.IsTesting ? reservation.Now : DateHelpers.GetLocalDateTime();
            var todayDate = DateHelpers.DateToISOString(today);
            return reservation.Date == todayDate;
        }

        private bool IsBetweenClosingTimeAndMidnight(ReservationWriteDto reservation) {
            var timeNow = testingEnvironment.IsTesting ? new TimeOnly(reservation.Now.Hour, reservation.Now.Minute) : new TimeOnly(DateHelpers.GetLocalDateTime().Hour, DateHelpers.GetLocalDateTime().Hour);
            var closingTime = new TimeOnly(context.ReservationParameters.Select(x => int.Parse(x.ClosingTime.Substring(0, 2))).SingleOrDefault(), context.ReservationParameters.Select(x => int.Parse(x.ClosingTime.Substring(3, 2))).SingleOrDefault());
            return (closingTime.Hour != 0 || closingTime.Minute != 0) && timeNow >= closingTime;
        }

        private async Task<bool> IsBetweenMidnightAndDeparture(ReservationWriteDto reservation) {
            var timeNow = testingEnvironment.IsTesting ? reservation.Now : DateHelpers.GetLocalDateTime();
            var departureTime = GetScheduleDepartureTimeAsync(reservation);
            return DateTime.Compare(timeNow, await departureTime) < 0;
        }

        private async Task<bool> IsAfterDeparture(ReservationWriteDto reservation) {
            var timeNow = testingEnvironment.IsTesting ? reservation.Now : DateHelpers.GetLocalDateTime();
            var departureTime = GetScheduleDepartureTimeAsync(reservation);
            return DateTime.Compare(timeNow, await departureTime) > 0;
        }

        private async Task<DateTime> GetScheduleDepartureTimeAsync(ReservationWriteDto reservation) {
            var portId = GetPortIdFromPickupPointId(reservation).ToString();
            var schedule = await context.Schedules
                .AsNoTracking()
                .Where(x => x.Date.ToString() == reservation.Date && x.DestinationId == reservation.DestinationId && x.PortId.ToString() == portId)
                .SingleOrDefaultAsync();
            var departureTime = schedule.Date.ToString("yyyy-MM-dd") + " " + schedule.Time;
            var departureTimeAsDate = DateTime.Parse(departureTime);
            return departureTimeAsDate;
        }

        private static bool IsFreePaxGreaterThanZero(ReservationWriteDto reservation, IEnumerable<AvailabilityGroupVM> availability) {
            var port = availability.Where(x => x.Date == reservation.Date)
                .SelectMany(x => x.Destinations).Where(x => x.Id == reservation.DestinationId)
                    .SelectMany(x => x.Ports).Where(x => x.Id == reservation.PortId).FirstOrDefault();
            var freePax = port.FreePax;
            var totalPax = reservation.Adults + reservation.Kids + reservation.Free;
            return freePax >= totalPax;
        }

        private async Task<bool> IsPaxLimitForSimpleUserExceededAsync(ReservationWriteDto reservation) {
            if (Identity.IsUserAdmin(httpContext) == false) {
                var x = await customerRepo.GetByIdAsync(reservation.CustomerId, false);
                if (x != null) {
                    var paxLimit = x.PaxLimit;
                    var existingPax = reservationCalculatePaxLimit.CalculateExistingPax(reservation.CustomerId, reservation.Date);
                    var pax = reservation.Adults + reservation.Kids + reservation.Free;
                    return existingPax + pax > paxLimit;
                } else {
                    throw new CustomException() {
                        ResponseCode = 404
                    };
                }
            } else {
                return false;
            }
        }

    }

}