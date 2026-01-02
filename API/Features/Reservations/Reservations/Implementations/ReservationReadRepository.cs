using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Features.Reservations.Drivers;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Infrastructure.Helpers;

namespace API.Features.Reservations.Reservations {

    public class ReservationReadRepository : Repository<Reservation>, IReservationReadRepository {

        private readonly IHttpContextAccessor httpContext;
        private readonly IMapper mapper;
        private readonly UserManager<UserExtended> userManager;

        public ReservationReadRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.httpContext = httpContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public IQueryable<ReservationListVM> GetByDateAsync(string date) {
            if (Identity.IsUserAdmin(httpContext)) {
                return GetReservationsFromAllUsersByDate(date);
            } else {
                var simpleUser = Identity.GetConnectedUserId(httpContext);
                var connectedUserDetails = Identity.GetConnectedUserDetails(userManager, simpleUser);
                return GetReservationsForLinkedCustomerAsync(date, (int)connectedUserDetails.CustomerId);
            }
        }

        public IQueryable<ReservationListVM> GetByRefNoAsync(string refNo) {
            if (Identity.IsUserAdmin(httpContext)) {
                return GetReservationsFromAllUsersByRefNoAsync(refNo);
            } else {
                var userId = Identity.GetConnectedUserId(httpContext);
                var userDetails = Identity.GetConnectedUserDetails(userManager, userId);
                return GetReservationsFromLinkedCustomerbyRefNoAsync(refNo, (int)userDetails.CustomerId);
            }
        }

        public async Task<ReservationDriverGroupVM> GetByDateAndDriverAsync(string date, int driverId) {
            var driver = await GetDriverAsync(driverId);
            var reservations = await GetReservationsByDateAndDriverAsync(date, driverId);
            return new ReservationDriverGroupVM {
                Date = date,
                DriverId = driver != null ? driverId : 0,
                DriverDescription = driver != null ? driver.Description : "(EMPTY)",
                Phones = driver != null ? driver.Phones : "(EMPTY)",
                Reservations = mapper.Map<IEnumerable<Reservation>, IEnumerable<ReservationDriverListVM>>(reservations)
            };
        }

        public async Task<Reservation> GetByIdAsync(string reservationId, bool includeTables) {
            return includeTables
                ? await context.Reservations
                    .AsNoTracking()
                    .Include(x => x.Customer)
                    .Include(x => x.PickupPoint).ThenInclude(y => y.CoachRoute)
                    .Include(x => x.Destination)
                    .Include(x => x.Driver)
                    .Include(x => x.Port)
                    .Include(x => x.PortAlternate)
                    .Include(x => x.Ship)
                    .Include(x => x.Passengers).ThenInclude(x => x.Gender)
                    .Include(x => x.Passengers).ThenInclude(x => x.Nationality)
                    .Where(x => x.ReservationId.ToString() == reservationId)
                    .SingleOrDefaultAsync()
               : await context.Reservations
                  .AsNoTracking()
                  .Include(x => x.Passengers)
                  .Where(x => x.ReservationId.ToString() == reservationId)
                  .SingleOrDefaultAsync();
        }

        public async Task<Reservation> GetByIdForPatchEmailSent(string reservationId) {
            return await context.Reservations
                .AsNoTracking()
                .Where(x => x.ReservationId.ToString() == reservationId)
                .SingleOrDefaultAsync();
        }

        private IQueryable<ReservationListVM> GetReservationsFromAllUsersByDate(string date) {
            return context.Reservations
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Destination)
                .Include(x => x.Driver)
                .Include(x => x.PickupPoint).ThenInclude(y => y.CoachRoute)
                .Include(x => x.Port)
                .Include(x => x.PortAlternate)
                .Include(x => x.Passengers)
                .Where(x => x.Date == Convert.ToDateTime(date)).Select(x => new ReservationListVM {
                    ReservationId = x.ReservationId,
                    LinkTwistId = x.LinkTwistId,
                    Date = DateHelpers.DateToISOString(x.Date),
                    RefNo = x.RefNo,
                    TicketNo = x.TicketNo,
                    Adults = x.Adults,
                    Kids = x.Kids,
                    Free = x.Free,
                    TotalPax = x.TotalPax,
                    Customer = new SimpleEntity {
                        Id = x.Customer.Id,
                        Description = x.Customer.Description
                    },
                    CoachRoute = new ReservationListCoachRouteVM {
                        Id = x.PickupPoint.CoachRoute.Id,
                        Abbreviation = x.PickupPoint.CoachRoute.Abbreviation
                    },
                    Destination = new ReservationListDestinationVM {
                        Id = x.Destination.Id,
                        Abbreviation = x.Destination.Abbreviation,
                        Description = x.Destination.Description
                    },
                    Driver = new ReservationListDriverVM {
                        Id = x.Driver != null ? x.Driver.Id : 0,
                        Description = x.Driver != null ? x.Driver.Description : "(EMPTY)",
                        Phones = x.Driver != null ? x.Driver.Phones : ""
                    },
                    PickupPoint = new ReservationListPickupPointVM {
                        Id = x.PickupPoint.Id,
                        Description = x.PickupPoint.Description,
                        Time = x.PickupPoint.Time
                    },
                    Port = new ReservationListPortVM {
                        Id = x.Port.Id,
                        Abbreviation = x.Port.Abbreviation,
                        Description = x.Port.Description
                    },
                    PortAlternate = new ReservationListPortVM {
                        Id = x.PortAlternate.Id,
                        Abbreviation = x.PortAlternate.Abbreviation,
                        Description = x.PortAlternate.Description
                    },
                    Ship = new ReservationListShipVM {
                        Id = x.Ship != null ? x.Ship.Id : 0,
                        Abbreviation = x.Ship != null ? x.Ship.Abbreviation : "(EMPTY)",
                        Description = x.Ship != null ? x.Ship.Description : "(EMPTY)"
                    },
                    PassengerCount = x.Passengers.Count,
                    PassengerDifference = x.TotalPax - x.Passengers.Count
                });
        }

        private IQueryable<ReservationListVM> GetReservationsForLinkedCustomerAsync(string date, int customerId) {
            return context.Reservations
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Destination)
                .Include(x => x.Driver)
                .Include(x => x.PickupPoint).ThenInclude(y => y.CoachRoute)
                .Include(x => x.Port)
                .Include(x => x.PortAlternate)
                .Include(x => x.Passengers)
                .Where(x => x.Date == Convert.ToDateTime(date) && x.CustomerId == customerId)
                .Select(x => new ReservationListVM {
                    ReservationId = x.ReservationId,
                    LinkTwistId = x.LinkTwistId,
                    Date = DateHelpers.DateToISOString(x.Date),
                    RefNo = x.RefNo,
                    TicketNo = x.TicketNo,
                    Adults = x.Adults,
                    Kids = x.Kids,
                    Free = x.Free,
                    TotalPax = x.TotalPax,
                    Customer = new SimpleEntity {
                        Id = x.Customer.Id,
                        Description = x.Customer.Description
                    },
                    CoachRoute = new ReservationListCoachRouteVM {
                        Id = x.PickupPoint.CoachRoute.Id,
                        Abbreviation = x.PickupPoint.CoachRoute.Abbreviation
                    },
                    Destination = new ReservationListDestinationVM {
                        Id = x.Destination.Id,
                        Abbreviation = x.Destination.Abbreviation,
                        Description = x.Destination.Description
                    },
                    Driver = new ReservationListDriverVM {
                        Id = x.Driver != null ? x.Driver.Id : 0,
                        Description = x.Driver != null ? x.Driver.Description : "(EMPTY)",
                        Phones = x.Driver != null ? x.Driver.Phones : ""
                    },
                    PickupPoint = new ReservationListPickupPointVM {
                        Id = x.PickupPoint.Id,
                        Description = x.PickupPoint.Description,
                        Time = x.PickupPoint.Time
                    },
                    Port = new ReservationListPortVM {
                        Id = x.Port.Id,
                        Abbreviation = x.Port.Abbreviation,
                        Description = x.Port.Description
                    },
                    PortAlternate = new ReservationListPortVM {
                        Id = x.PortAlternate.Id,
                        Abbreviation = x.PortAlternate.Abbreviation,
                        Description = x.PortAlternate.Description
                    },
                    Ship = new ReservationListShipVM {
                        Id = x.Ship != null ? x.Ship.Id : 0,
                        Abbreviation = x.Ship != null ? x.Ship.Abbreviation : "(EMPTY)",
                        Description = x.Ship != null ? x.Ship.Description : "(EMPTY)"
                    },
                    PassengerCount = x.Passengers.Count,
                    PassengerDifference = x.TotalPax - x.Passengers.Count
                });
        }

        private IQueryable<ReservationListVM> GetReservationsFromAllUsersByRefNoAsync(string refNo) {
            return context.Reservations
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Destination)
                .Include(x => x.Driver)
                .Include(x => x.PickupPoint).ThenInclude(y => y.CoachRoute)
                .Include(x => x.Port)
                .Include(x => x.PortAlternate)
                .Include(x => x.Passengers)
                .Where(x => x.RefNo == refNo || x.TicketNo == refNo).Select(x => new ReservationListVM {
                    ReservationId = x.ReservationId,
                    LinkTwistId = x.LinkTwistId,
                    Date = DateHelpers.DateToISOString(x.Date),
                    RefNo = x.RefNo,
                    TicketNo = x.TicketNo,
                    Adults = x.Adults,
                    Kids = x.Kids,
                    Free = x.Free,
                    TotalPax = x.TotalPax,
                    Customer = new SimpleEntity {
                        Id = x.Customer.Id,
                        Description = x.Customer.Description
                    },
                    CoachRoute = new ReservationListCoachRouteVM {
                        Id = x.PickupPoint.CoachRoute.Id,
                        Abbreviation = x.PickupPoint.CoachRoute.Abbreviation
                    },
                    Destination = new ReservationListDestinationVM {
                        Id = x.Destination.Id,
                        Abbreviation = x.Destination.Abbreviation,
                        Description = x.Destination.Description
                    },
                    Driver = new ReservationListDriverVM {
                        Id = x.Driver != null ? x.Driver.Id : 0,
                        Description = x.Driver != null ? x.Driver.Description : "(EMPTY)",
                        Phones = x.Driver != null ? x.Driver.Phones : ""
                    },
                    PickupPoint = new ReservationListPickupPointVM {
                        Id = x.PickupPoint.Id,
                        Description = x.PickupPoint.Description,
                        Time = x.PickupPoint.Time
                    },
                    Port = new ReservationListPortVM {
                        Id = x.Port.Id,
                        Abbreviation = x.Port.Abbreviation,
                        Description = x.Port.Description
                    },
                    PortAlternate = new ReservationListPortVM {
                        Id = x.PortAlternate.Id,
                        Abbreviation = x.PortAlternate.Abbreviation,
                        Description = x.PortAlternate.Description
                    },
                    Ship = new ReservationListShipVM {
                        Id = x.Ship != null ? x.Ship.Id : 0,
                        Abbreviation = x.Ship != null ? x.Ship.Abbreviation : "(EMPTY)",
                        Description = x.Ship != null ? x.Ship.Description : "(EMPTY)"
                    },
                    PassengerCount = x.Passengers.Count,
                    PassengerDifference = x.TotalPax - x.Passengers.Count
                });
        }

        private IQueryable<ReservationListVM> GetReservationsFromLinkedCustomerbyRefNoAsync(string refNo, int customerId) {
            return context.Reservations
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Destination)
                .Include(x => x.Driver)
                .Include(x => x.PickupPoint).ThenInclude(y => y.CoachRoute)
                .Include(x => x.Port)
                .Include(x => x.PortAlternate)
                .Include(x => x.Passengers)
                .Where(x => (x.RefNo == refNo || x.TicketNo == refNo) && x.CustomerId == customerId).Select(x => new ReservationListVM {
                    ReservationId = x.ReservationId,
                    LinkTwistId = x.LinkTwistId,
                    Date = DateHelpers.DateToISOString(x.Date),
                    RefNo = x.RefNo,
                    TicketNo = x.TicketNo,
                    Adults = x.Adults,
                    Kids = x.Kids,
                    Free = x.Free,
                    TotalPax = x.TotalPax,
                    Customer = new SimpleEntity {
                        Id = x.Customer.Id,
                        Description = x.Customer.Description
                    },
                    CoachRoute = new ReservationListCoachRouteVM {
                        Id = x.PickupPoint.CoachRoute.Id,
                        Abbreviation = x.PickupPoint.CoachRoute.Abbreviation
                    },
                    Destination = new ReservationListDestinationVM {
                        Id = x.Destination.Id,
                        Abbreviation = x.Destination.Abbreviation,
                        Description = x.Destination.Description
                    },
                    Driver = new ReservationListDriverVM {
                        Id = x.Driver != null ? x.Driver.Id : 0,
                        Description = x.Driver != null ? x.Driver.Description : "(EMPTY)",
                        Phones = x.Driver != null ? x.Driver.Phones : ""
                    },
                    PickupPoint = new ReservationListPickupPointVM {
                        Id = x.PickupPoint.Id,
                        Description = x.PickupPoint.Description,
                        Time = x.PickupPoint.Time
                    },
                    Port = new ReservationListPortVM {
                        Id = x.Port.Id,
                        Abbreviation = x.Port.Abbreviation,
                        Description = x.Port.Description
                    },
                    PortAlternate = new ReservationListPortVM {
                        Id = x.PortAlternate.Id,
                        Abbreviation = x.PortAlternate.Abbreviation,
                        Description = x.PortAlternate.Description
                    },
                    Ship = new ReservationListShipVM {
                        Id = x.Ship != null ? x.Ship.Id : 0,
                        Abbreviation = x.Ship != null ? x.Ship.Abbreviation : "(EMPTY)",
                        Description = x.Ship != null ? x.Ship.Description : "(EMPTY)"
                    },
                    PassengerCount = x.Passengers.Count,
                    PassengerDifference = x.TotalPax - x.Passengers.Count
                });
        }

        private async Task<IEnumerable<Reservation>> GetReservationsByDateAndDriverAsync(string date, int driverId) {
            return await context.Reservations
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Destination)
                .Include(x => x.Driver)
                .Include(x => x.PickupPoint)
                .Include(x => x.Passengers)
                .Where(x => x.Date == Convert.ToDateTime(date) && x.DriverId == (driverId != 0 ? driverId : null))
                .OrderBy(x => x.PickupPoint.Time).ThenBy(x => x.PickupPoint.Description)
                .ToListAsync();
        }

        private async Task<Driver> GetDriverAsync(int driverId) {
            return await context.Drivers
                .AsNoTracking()
                .Where(x => x.Id == driverId)
                .SingleOrDefaultAsync();
        }

    }

}