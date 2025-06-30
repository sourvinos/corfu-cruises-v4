using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.PaymentMethods {

    public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository {

        private readonly IMapper mapper;

        public PaymentMethodRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PaymentMethodListVM>> GetAsync() {
            var paymentMethods = await context.PaymentMethods
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<PaymentMethod>, IEnumerable<PaymentMethodListVM>>(paymentMethods);
        }

        public async Task<IEnumerable<PaymentMethodBrowserVM>> GetForBrowserAsync() {
            var paymentMethods = await context.PaymentMethods
                .AsNoTracking()
                .OrderBy(x => x.DescriptionEn)
                .ToListAsync();
            return mapper.Map<IEnumerable<PaymentMethod>, IEnumerable<PaymentMethodBrowserVM>>(paymentMethods);
        }

        public async Task<PaymentMethod> GetByIdAsync(string id) {
            return await context.PaymentMethods
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id.ToString() == id);
        }

    }

}