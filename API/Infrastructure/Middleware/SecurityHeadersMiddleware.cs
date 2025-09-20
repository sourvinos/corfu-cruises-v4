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
            context.Response.Headers.Append("X-XSS-Protection", new StringValues("1; mode=block"));
            context.Response.Headers.Append("X-Frame-Options", new StringValues("DENY"));
            context.Response.Headers.Append("Referrer-Policy", new StringValues("no-referrer-when-downgrade"));
            context.Response.Headers.Append("X-Content-Type-Options", new StringValues("nosniff"));
            context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", new StringValues("none"));
            context.Response.Headers.Append("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
            context.Response.Headers.Append("Content-Security-Policy", new StringValues("font-src https://appsourvinos.com https://fonts.cdnfonts.com fonts.googleapis.com fonts.gstatic.com"));
            await _next(context);
        }
    }

}