using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using API.Infrastructure.Implementations;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;

namespace API.Features.Reservations.Reservations {

    public class ReservationCalculatePaxLimit : Repository<Reservation>, IReservationCalculatePaxLimit {

        public ReservationCalculatePaxLimit(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int CalculateExistingPax(int customerId, string date) {
            var x = context.Reservations
                .AsNoTracking()
                .Include(x => x.Customer)
                .Where(x => x.CustomerId == customerId && x.Date == DateHelpers.StringToDate(date))
                .OrderBy(x => x.Date)
                .Sum(x => x.TotalPax);
            return x;
        }

    }

}