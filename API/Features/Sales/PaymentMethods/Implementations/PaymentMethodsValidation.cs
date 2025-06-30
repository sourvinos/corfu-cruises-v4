using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.PaymentMethods {

    public class PaymentMethodValidation : Repository<PaymentMethod>, IPaymentMethodValidation {

        public PaymentMethodValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(PaymentMethod z, PaymentMethodWriteDto paymentMethod) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, paymentMethod) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(PaymentMethod z, PaymentMethodWriteDto paymentMethod) {
            return z != null && z.PutAt != paymentMethod.PutAt;
        }

    }

}