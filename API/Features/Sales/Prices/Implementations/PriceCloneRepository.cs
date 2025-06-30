using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.Prices {

    public class PriceCloneRepository : Repository<Price>, IPriceCloneRepository {

        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<UserExtended> userManager;

        public PriceCloneRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(context, httpContextAccessor, settings, userManager) {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public PriceWriteDto BuildPriceWriteDto(int customerId, Price price) {
            return new PriceWriteDto {
                CustomerId = customerId,
                DestinationId = price.DestinationId,
                PortId = price.PortId,
                From = price.From.ToString(),
                To = price.To.ToString(),
                AdultsWithTransfer = price.AdultsWithTransfer,
                AdultsWithoutTransfer = price.AdultsWithoutTransfer,
                KidsWithTransfer = price.KidsWithTransfer,
                KidsWithoutTransfer = price.KidsWithoutTransfer
            };
        }

        public async void ClonePricesAsync(PriceCloneCriteria criteria) {
            var postAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime());
            var postUser = Identity.GetConnectedUserDetails(userManager, Identity.GetConnectedUserId(httpContextAccessor)).UserName;
            foreach (var customerId in criteria.CustomerIds) {
                foreach (var priceId in criteria.PriceIds) {
                    var z = await context.Prices.FirstOrDefaultAsync(x => x.Id.ToString() == priceId);
                    var x = new PriceWriteDto {
                        CustomerId = customerId,
                        DestinationId = z.DestinationId,
                        PortId = z.PortId,
                        From = z.From.ToString(),
                        To = z.To.ToString(),
                        AdultsWithTransfer = z.AdultsWithTransfer,
                        AdultsWithoutTransfer = z.AdultsWithoutTransfer,
                        KidsWithTransfer = z.KidsWithTransfer,
                        KidsWithoutTransfer = z.KidsWithoutTransfer,
                        PostAt = postAt,
                        PostUser = postUser,
                        PutAt = postAt,
                        PutUser = postUser
                    };
                    await context.Prices.AddAsync(mapper.Map<PriceWriteDto, Price>(x));
                    await context.SaveChangesAsync();
                }
            }
        }

    }

}