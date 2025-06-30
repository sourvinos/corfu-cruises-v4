using API.Infrastructure.Users;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Account {

    [Route("api/[controller]")]
    public class AccountController : Controller {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserRepository userRepo;
        private readonly SignInManager<UserExtended> signInManager;
        private readonly UserManager<UserExtended> userManager;

        public AccountController(IHttpContextAccessor httpContextAccessor, IUserRepository userRepo, SignInManager<UserExtended> signInManager, UserManager<UserExtended> userManager) {
            this.httpContextAccessor = httpContextAccessor;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userRepo = userRepo;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<Response> ResetPassword([FromBody] ResetPasswordVM model) {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null) {
                var result = await userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token)), model.Password);
                if (result.Succeeded) {
                    await signInManager.RefreshSignInAsync(user);
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = null,
                        Message = ApiMessages.OK()
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = 412
                    };
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "user, admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> ChangePassword([FromBody] ChangePasswordVM changePassword) {
            var user = await userManager.FindByIdAsync(changePassword.UserId);
            if (user != null) {
                var result = await userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.Password);
                if (result.Succeeded) {
                    await signInManager.RefreshSignInAsync(user);
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = null,
                        Message = ApiMessages.OK()
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = 412
                    };
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [AllowAnonymous]
        [HttpPatch("[action]")]
        public async Task<Response> PatchUserWithResetEmailPending([FromBody] ForgotPasswordRequestVM form) {
            var x = await userManager.FindByEmailAsync(form.Email);
            if (x != null) {
                await userRepo.UpdateIsResetPasswordEmailPendingAsync(x);
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
            return new Response {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK()
            };
        }

        [HttpGet("[action]")]
        [Authorize]
        public string GetConnectedUserId() {
            return Identity.GetConnectedUserId(httpContextAccessor);
        }

        [Authorize]
        [HttpGet("[action]")]
        public bool IsConnectedUserAdmin() {
            return Identity.IsUserAdmin(httpContextAccessor);
        }

    }

}