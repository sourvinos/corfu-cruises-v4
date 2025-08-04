using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace API.Infrastructure.Middleware {

    public class SecurityHeadersMiddleware {

        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            context.Response.Headers.Add("Content-Security-Policy", new StringValues("font-src https://appsourvinos.com https://fonts.cdnfonts.com fonts.googleapis.com fonts.gstatic.com"));
            context.Response.Headers.Add("X-Content-Type-Options", new StringValues("nosniff"));
            context.Response.Headers.Add("X-Frame-Options", new StringValues("SAMEORIGIN"));
            context.Response.Headers.Add("Referrer-Policy", new StringValues("no-referrer-when-downgrade"));
            context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
            await _next(context);
        }
    }

}