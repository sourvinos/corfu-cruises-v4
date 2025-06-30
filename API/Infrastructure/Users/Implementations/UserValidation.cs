using System.Linq;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Users {

    public class UserValidation : IUserValidation<IUser> {

        #region variables

        private readonly AppDbContext context;
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<UserExtended> userManager;

        #endregion

        public UserValidation(AppDbContext context, IHttpContextAccessor httpContext, UserManager<UserExtended> userManager) {
            this.context = context;
            this.httpContext = httpContext;
            this.userManager = userManager;
        }

        public int IsValid(IUser user) {
            return true switch {
                var x when x == !AdminShouldNotHaveCustomerId(user) => 416,
                var x when x == !SimpleUserShouldHaveCustomerId(user) => 417,
                var x when x == !SimpleUserShouldHaveActiveCustomerId(user) => 418,
                _ => 200,
            };
        }

        public bool IsUserOwner(string userId) {
            var connectedUserId = Identity.GetConnectedUserId(httpContext);
            var connectedUserDetails = Identity.GetConnectedUserDetails(userManager, userId);
            return connectedUserDetails.Id == connectedUserId;
        }

        private static bool AdminShouldNotHaveCustomerId(IUser user) {
            if (user.IsAdmin) {
                if (user.CustomerId == null || user.CustomerId == 0) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }

        private static bool SimpleUserShouldHaveCustomerId(IUser user) {
            if (!user.IsAdmin) {
                if (user.CustomerId == null || user.CustomerId == 0) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return true;
            }
        }

        private bool SimpleUserShouldHaveActiveCustomerId(IUser user) {
            var customer = context.Customers
                .AsNoTracking()
                .SingleOrDefault(x => x.Id == user.CustomerId && x.IsActive) != null;
            if (!user.IsAdmin && !customer) {
                return false;
            } else {
                return true;
            }
        }

    }

}
