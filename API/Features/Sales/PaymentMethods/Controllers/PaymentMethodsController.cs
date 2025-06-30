using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.PaymentMethods {

    [Route("api/[controller]")]
    public class PaymentMethodsController : ControllerBase {

        #region variables

        private readonly IPaymentMethodRepository paymentMethodRepo;
        private readonly IPaymentMethodValidation paymentMethodValidation;
        private readonly IMapper mapper;

        #endregion

        public PaymentMethodsController(IPaymentMethodRepository paymentMethodRepo, IPaymentMethodValidation paymentMethodValidation, IMapper mapper) {
            this.paymentMethodRepo = paymentMethodRepo;
            this.paymentMethodValidation = paymentMethodValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<PaymentMethodListVM>> GetAsync() {
            return await paymentMethodRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<PaymentMethodBrowserVM>> GetForBrowserAsync() {
            return await paymentMethodRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string id) {
            var x = await paymentMethodRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<PaymentMethod, PaymentMethodReadDto>(x)
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public Response Post([FromBody] PaymentMethodWriteDto paymentMethod) {
            var x = paymentMethodValidation.IsValid(null, paymentMethod);
            if (x == 200) {
                var z = paymentMethodRepo.Create(mapper.Map<PaymentMethodWriteDto, PaymentMethod>((PaymentMethodWriteDto)paymentMethodRepo.AttachMetadataToPostDto(paymentMethod)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = z.Id.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = x
                };
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> Put([FromBody] PaymentMethodWriteDto paymentMethod) {
            var x = await paymentMethodRepo.GetByIdAsync(paymentMethod.Id.ToString());
            if (x != null) {
                var z = paymentMethodValidation.IsValid(x, paymentMethod);
                if (z == 200) {
                    paymentMethodRepo.Update(mapper.Map<PaymentMethodWriteDto, PaymentMethod>((PaymentMethodWriteDto)paymentMethodRepo.AttachMetadataToPutDto(x, paymentMethod)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = x.Id.ToString(),
                        Message = ApiMessages.OK()
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = z
                    };
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<Response> Delete([FromRoute] string id) {
            var x = await paymentMethodRepo.GetByIdAsync(id);
            if (x != null) {
                paymentMethodRepo.Delete(x);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.Id.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

    }

}