using System;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace API.Infrastructure.Middleware {

    public class ResponseMiddleware : IMiddleware {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<UserExtended> userManager;

        public ResponseMiddleware(IHttpContextAccessor httpContextAccessor, UserManager<UserExtended> userManager) {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next) {
            try {
                await next(httpContext);
            }
            catch (CustomException exception) {
                await CreateCustomErrorResponse(httpContext, exception);
            }
            catch (DbUpdateConcurrencyException exception) {
                await CreateConcurrencyErrorResponse(httpContext, exception);
            }
            catch (Exception exception) {
                if (exception.Message.Contains("boo.com")) {
                    await CreateCustomHttpErrorResponse(httpContext, exception);
                } else {
                    LogError(exception, httpContextAccessor, userManager);
                    await CreateServerErrorResponse(httpContext, exception);
                }
            }
        }

        private static Task CreateCustomHttpErrorResponse(HttpContext httpContext, Exception exception) {
            httpContext.Response.StatusCode = 501;
            httpContext.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new Response {
                Code = 501,
                Icon = Icons.Error.ToString(),
                Id = null,
                Message = GetErrorMessage(501)
            });
            return httpContext.Response.WriteAsync(result);
        }

        private static Task CreateCustomErrorResponse(HttpContext httpContext, CustomException e) {
            httpContext.Response.StatusCode = e.ResponseCode;
            httpContext.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new Response {
                Code = e.ResponseCode,
                Icon = Icons.Error.ToString(),
                Id = null,
                Message = GetErrorMessage(e.ResponseCode)
            });
            return httpContext.Response.WriteAsync(result);
        }

        private static Task CreateConcurrencyErrorResponse(HttpContext httpContext, DbUpdateConcurrencyException exception) {
            httpContext.Response.StatusCode = 415;
            httpContext.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new Response {
                Code = 415,
                Icon = Icons.Error.ToString(),
                Id = null,
                Message = GetErrorMessage(httpContext.Response.StatusCode)
            });
            return httpContext.Response.WriteAsync(result);
        }

        private static Task CreateServerErrorResponse(HttpContext httpContext, Exception e) {
            httpContext.Response.StatusCode = 500;
            httpContext.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new Response {
                Code = 500,
                Icon = Icons.Error.ToString(),
                Id = null,
                Message = e.Message
            });
            return httpContext.Response.WriteAsync(result);
        }

        private static string GetErrorMessage(int httpResponseCode) {
            return httpResponseCode switch {
                401 => ApiMessages.AuthenticationFailed(),
                402 => ApiMessages.AadeError(),
                403 => ApiMessages.CheckInAfterDepartureIsNotAllowed(),
                404 => ApiMessages.RecordNotFound(),
                405 => ApiMessages.InvalidIssueDate(),
                406 => ApiMessages.InvalidBank(),
                407 => ApiMessages.VatNumberIsDuplicate(),
                408 => ApiMessages.InvalidCoachRoute(),
                409 => ApiMessages.DuplicateRecord(),
                410 => ApiMessages.InvalidDateDestinationOrPickupPoint(),
                411 => ApiMessages.InvalidPort(),
                412 => ApiMessages.InvalidAccountFields(),
                413 => ApiMessages.CustomerIdDoesNotMatchConnectedSimpleUserCustomerId(),
                414 => ApiMessages.DuplicateRefNo(),
                415 => ApiMessages.ConcurrencyError(),
                416 => ApiMessages.NewAdminShouldNotHaveCustomerId(),
                417 => ApiMessages.NewSimpleUserShouldHaveCustomerId(),
                418 => ApiMessages.NewSimpleUserShouldHaveCustomerId(),
                419 => ApiMessages.PriceCloningNotCompleted(),
                420 => ApiMessages.OxygenOkAadeFault(),
                431 => ApiMessages.SimpleUserCanNotAddReservationAfterDepartureTime(),
                433 => ApiMessages.PortHasNoFreeSeats(),
                449 => ApiMessages.InvalidShipOwner(),
                450 => ApiMessages.InvalidCustomer(),
                451 => ApiMessages.InvalidDestination(),
                452 => ApiMessages.InvalidPickupPoint(),
                453 => ApiMessages.InvalidDriver(),
                454 => ApiMessages.InvalidShip(),
                455 => ApiMessages.InvalidPassengerCount(),
                456 => ApiMessages.InvalidNationality(),
                457 => ApiMessages.InvalidGender(),
                458 => ApiMessages.InvalidTaxOffice(),
                459 => ApiMessages.SimpleUserNightRestrictions(),
                460 => ApiMessages.InvalidPort(),
                461 => ApiMessages.PriceFieldsMustBeZeroOrGreater(),
                468 => ApiMessages.CustomerPaxFieldMustBeZeroOrGreater(),
                462 => ApiMessages.InvalidDatePeriod(),
                463 => ApiMessages.InvoiceIsAlreadySaved(),
                464 => ApiMessages.InvalidCrewSpecialty(),
                465 => ApiMessages.InvalidDocumentType(),
                466 => ApiMessages.TransactionCompositeIndexIsInvalid(),
                467 => ApiMessages.TransactionCountMismatch(),
                469 => ApiMessages.CustomerPaxLimitIsExceeded(),
                490 => ApiMessages.NotOwnRecord(),
                491 => ApiMessages.RecordInUse(),
                492 => ApiMessages.NotUniqueUsernameOrEmail(),
                493 => ApiMessages.InvalidPortOrder(),
                498 => ApiMessages.EmailNotSent(),
                501 => ApiMessages.OxygenNotResponding(),
                _ => ApiMessages.UnknownError(),
            };
        }

        private static void LogError(Exception exception, IHttpContextAccessor httpContextAccessor, UserManager<UserExtended> userManager) {
            Log.Error("USER {userId} | MESSAGE {message}", Identity.GetConnectedUserDetails(userManager, Identity.GetConnectedUserId(httpContextAccessor)).UserName, exception.Message);
        }

    }

}