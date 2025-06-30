using System;
using System.Linq;
using System.Security.Claims;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace API.Infrastructure.Extensions {

    public static class Identity {

        public static void AddIdentity(IServiceCollection services) {
            services
                .AddIdentity<UserExtended, IdentityRole>(options => {
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 10;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services
                .Configure<DataProtectionTokenProviderOptions>(options => {
                    options.TokenLifespan = TimeSpan.FromMinutes(5);
                });
        }

        public static string GetConnectedUserId(IHttpContextAccessor httpContextAccessor) {
            return httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public static UserExtended GetConnectedUserDetails(UserManager<UserExtended> userManager, string userId) {
            return userManager.Users.SingleOrDefault(x => x.Id == userId);
        }

        public static bool IsUserAdmin(IHttpContextAccessor httpContextAccessor) {
            try {
                return httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value == "admin";
            }
            catch (Exception) {
                return false;
            }
        }

    }

}