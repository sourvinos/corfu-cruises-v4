using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.Prices {

    public class PriceValidation : Repository<Price>, IPriceValidation {

        public PriceValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(context, httpContext, settings, userManager) { }

        public int IsValid(Price z, PriceWriteDto price) {
            return true switch {
                var x when x == !IsValidCustomer(price) => 450,
                var x when x == !IsValidDestination(price) => 451,
                var x when x == !IsValidPort(price) => 460,
                var x when x == !IsValidDatePeriod(price) => 462,
                var x when x == !PriceFieldsMustBeZeroOrGreater(price) => 461,
                var x when x == IsAlreadyUpdated(z, price) => 415,
                _ => 200,
            };
        }

        private bool IsValidCustomer(PriceWriteDto price) {
            return price.Id == 0
                ? context.Customers
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == price.CustomerId && x.IsActive) != null
                : context.Customers
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == price.CustomerId) != null;
        }

        private bool IsValidDestination(PriceWriteDto price) {
            return price.Id == 0
                ? context.Destinations
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == price.DestinationId && x.IsActive) != null
                : context.Destinations
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == price.DestinationId) != null;
        }

        private bool IsValidPort(PriceWriteDto price) {
            return price.Id == 0
                ? context.Ports
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == price.PortId && x.IsActive) != null
                : context.Ports
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == price.PortId) != null;
        }

        private static bool IsValidDatePeriod(PriceWriteDto price) {
            return DateHelpers.StringToDate(price.To) >= DateHelpers.StringToDate(price.From);
        }

        private static bool PriceFieldsMustBeZeroOrGreater(PriceWriteDto price) {
            return price.AdultsWithTransfer >= 0 && price.AdultsWithoutTransfer >= 0 && price.KidsWithTransfer >= 0 && price.KidsWithoutTransfer >= 0;
        }

        private static bool IsAlreadyUpdated(Price z, PriceWriteDto price) {
            return z != null && z.PutAt != price.PutAt;
        }

    }

}