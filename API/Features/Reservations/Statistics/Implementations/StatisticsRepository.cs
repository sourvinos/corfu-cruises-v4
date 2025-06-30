using System;
using System.Collections.Generic;
using System.Linq;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Statistics {

    public class StatisticsRepository : Repository<Reservation>, IStatisticsRepository {


        public StatisticsRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public IEnumerable<StatisticsVM> Get(int year) {
            var x = context.Reservations
                .AsNoTracking()
                .Include(x => x.Passengers)
                .Where(x => x.Date >= new DateTime(year, 1, 1) && x.Date <= new DateTime(year, DateHelpers.GetLocalDateTime().Month, DateHelpers.GetLocalDateTime().Day))
                .GroupBy(x => new { x.Date.Year })
                .Select(x => new StatisticsVM {
                    Pax = x.Sum(x => x.TotalPax),
                    ActualPax = x.Sum(x => x.Passengers.Where(x => x.IsBoarded == true).Count()),
                }).OrderByDescending(x => x.ActualPax).ToList();
            x.Add(new StatisticsVM {
                Description = "",
                Pax = x.Sum(x => x.Pax),
                ActualPax = x.Sum(x => x.ActualPax)
            });
            return x;
        }

        public IEnumerable<StatisticsVM> GetPerDestination(int year) {
            var x = context.Reservations
                .AsNoTracking()
                .Include(x => x.Passengers)
                .Where(x => x.Date >= new DateTime(year, 1, 1) && x.Date <= new DateTime(year, DateHelpers.GetLocalDateTime().Month, DateHelpers.GetLocalDateTime().Day))
                .GroupBy(x => new { x.Date.Year, x.Destination.Id, x.Destination.Description })
                .OrderBy(x => x.Key.Description)
                .Select(x => new StatisticsVM {
                    Id = x.Key.Id,
                    Description = x.Key.Description,
                    Pax = x.Sum(x => x.TotalPax),
                    ActualPax = x.Sum(x => x.Passengers.Where(x => x.IsBoarded == true).Count()),
                }).OrderByDescending(x => x.ActualPax).ToList();
            x.Add(new StatisticsVM {
                Description = "",
                Pax = x.Sum(x => x.Pax),
                ActualPax = x.Sum(x => x.ActualPax)
            });
            return x;
        }

        public IEnumerable<StatisticsVM> GetPerCustomer(int year) {
            var x = context.Reservations
                .AsNoTracking()
                .Include(x => x.Passengers)
                .Where(x => x.Date >= new DateTime(year, 1, 1) && x.Date <= new DateTime(year, DateHelpers.GetLocalDateTime().Month, DateHelpers.GetLocalDateTime().Day))
                .GroupBy(x => new { x.Customer.Id, x.Customer.Description })
                .OrderBy(x => x.Key.Description)
                .Select(x => new StatisticsVM {
                    Id = x.Key.Id,
                    Description = x.Key.Description,
                    Pax = x.Sum(x => x.TotalPax),
                    ActualPax = x.Sum(x => x.Passengers.Where(x => x.IsBoarded == true).Count())
                }).OrderByDescending(x => x.ActualPax).ToList();
            x.Add(new StatisticsVM {
                Description = "",
                Pax = x.Sum(x => x.Pax),
                ActualPax = x.Sum(x => x.ActualPax)
            });
            return x;
        }

        public IEnumerable<StatisticsVM> GetPerDriver(int year) {
            var x = context.Reservations
                .AsNoTracking()
                .Include(x => x.Passengers)
                .Where(x => x.Date >= new DateTime(year, 1, 1) && x.Date <= new DateTime(year, DateHelpers.GetLocalDateTime().Month, DateHelpers.GetLocalDateTime().Day) && x.DriverId != null)
                .GroupBy(x => new { x.Date.Year, x.Driver.Id, x.Driver.Description })
                .OrderBy(x => x.Key.Description)
                .Select(x => new StatisticsVM {
                    Id = x.Key.Id,
                    Description = x.Key.Description,
                    Pax = x.Sum(x => x.TotalPax),
                    ActualPax = x.Sum(x => x.Passengers.Where(x => x.IsBoarded == true).Count()),
                }).OrderByDescending(x => x.ActualPax).ToList();
            x.Add(new StatisticsVM {
                Description = "",
                Pax = x.Sum(x => x.Pax),
                ActualPax = x.Sum(x => x.ActualPax)
            });
            return x;
        }

        public IEnumerable<StatisticsVM> GetPerPort(int year) {
            var x = context.Reservations
                .AsNoTracking()
                .Include(x => x.Passengers)
                .Where(x => x.Date >= new DateTime(year, 1, 1) && x.Date <= new DateTime(year, DateHelpers.GetLocalDateTime().Month, DateHelpers.GetLocalDateTime().Day))
                .GroupBy(x => new { x.Date.Year, x.Port.Id, x.Port.Description })
                .OrderBy(x => x.Key.Description)
                .Select(x => new StatisticsVM {
                    Id = x.Key.Id,
                    Description = x.Key.Description,
                    Pax = x.Sum(x => x.TotalPax),
                    ActualPax = x.Sum(x => x.Passengers.Where(x => x.IsBoarded == true).Count()),
                }).OrderByDescending(x => x.ActualPax).ToList();
            x.Add(new StatisticsVM {
                Description = "",
                Pax = x.Sum(x => x.Pax),
                ActualPax = x.Sum(x => x.ActualPax)
            });
            return x;
        }

        public IEnumerable<StatisticsVM> GetPerShip(int year) {
            var x = context.Reservations
                .AsNoTracking()
                .Include(x => x.Passengers)
                .Where(x => x.Date >= new DateTime(year, 1, 1) && x.Date <= new DateTime(year, DateHelpers.GetLocalDateTime().Month, DateHelpers.GetLocalDateTime().Day) && x.ShipId != null)
                .GroupBy(x => new { x.Date.Year, x.Ship.Id, x.Ship.Description })
                .OrderBy(x => x.Key.Description)
                .Select(x => new StatisticsVM {
                    Id = x.Key.Id,
                    Description = x.Key.Description,
                    Pax = x.Sum(x => x.TotalPax),
                    ActualPax = x.Sum(x => x.Passengers.Where(x => x.IsBoarded == true).Count()),
                }).OrderByDescending(x => x.ActualPax).ToList();
            x.Add(new StatisticsVM {
                Description = "",
                Pax = x.Sum(x => x.Pax),
                ActualPax = x.Sum(x => x.ActualPax)
            });
            return x;
        }

        public IEnumerable<StatisticsUserVM> GetPerUser(int year) {
            var x = context.Reservations
                .AsNoTracking()
                .Where(x => x.Date >= new DateTime(year, 1, 1) && x.Date <= new DateTime(year, DateHelpers.GetLocalDateTime().Month, DateHelpers.GetLocalDateTime().Day))
                .GroupBy(x => new { x.Date.Year, x.PostUser })
                .Select(x => new StatisticsUserVM {
                    PostUser = x.Key.PostUser,
                    Reservations = x.Count()
                }).OrderByDescending(x => x.Reservations).ToList();
            x.Add(new StatisticsUserVM {
                PostUser = "",
                Reservations = x.Count
            });
            return x;
        }

        public IEnumerable<StatisticsNationalityVM> GetPerNationality(int year) {
            var x = context.Reservations
                .AsNoTracking()
                .Include(x => x.Passengers)
                .Where(x => x.Date >= new DateTime(year, 1, 1) && x.Date <= new DateTime(year, DateHelpers.GetLocalDateTime().Month, DateHelpers.GetLocalDateTime().Day))
                .SelectMany(x => x.Passengers)
                .GroupBy(x => new { x.NationalityId, x.Nationality.Code, x.Nationality.Description })
                .OrderBy(x => x.Key.Description)
                .Select(x => new StatisticsNationalityVM {
                    Id = x.Key.NationalityId,
                    Code = x.Key.Code,
                    Description = x.Key.Description,
                    Pax = x.Count(),
                    ActualPax = x.Count(x => x.IsBoarded),
                }).OrderByDescending(x => x.ActualPax).ToList();
            x.Add(new StatisticsNationalityVM {
                Description = "",
                Pax = x.Sum(x => x.Pax),
                ActualPax = x.Sum(x => x.ActualPax)
            });
            return x;
        }

    }

}
