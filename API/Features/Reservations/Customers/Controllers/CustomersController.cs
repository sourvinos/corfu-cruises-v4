using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Customers {

    [Route("api/[controller]")]
    public class CustomersController : ControllerBase {

        #region variables

        private readonly ICustomerRepository customerRepo;
        private readonly ICustomerValidation customerValidation;
        private readonly IMapper mapper;

        #endregion

        public CustomersController(ICustomerRepository customerRepo, ICustomerValidation customerValidation, IMapper mapper) {
            this.customerRepo = customerRepo;
            this.customerValidation = customerValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<CustomerListVM>> GetAsync() {
            return await customerRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<CustomerBrowserVM>> GetForBrowserAsync() {
            return await customerRepo.GetForBrowserAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            return await customerRepo.GetForCriteriaAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await customerRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Customer, CustomerReadDto>(x)
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
        public async Task<ResponseWithBody> PostAsync([FromBody] CustomerWriteDto customer) {
            var x = customerValidation.IsValidAsync(null, customer);
            if (await x == 200) {
                var isValidWithWarnings = await customerValidation.IsValidWithWarningAsync(customer);
                var z = customerRepo.Create(mapper.Map<CustomerWriteDto, Customer>((CustomerWriteDto)customerRepo.AttachMetadataToPostDto(customer)));
                return new ResponseWithBody {
                    Code = isValidWithWarnings,
                    Icon = Icons.Success.ToString(),
                    Body = customerRepo.GetByIdForBrowserAsync(z.Id).Result,
                    Message = isValidWithWarnings == 200 ? ApiMessages.OK() : ApiMessages.VatNumberIsDuplicate()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = await x
                };
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<ResponseWithBody> PutAsync([FromBody] CustomerWriteDto customer) {
            var x = await customerRepo.GetByIdAsync(customer.Id, false);
            if (x != null) {
                var z = customerValidation.IsValidAsync(x, customer);
                if (await z == 200) {
                    customerRepo.Update(mapper.Map<CustomerWriteDto, Customer>((CustomerWriteDto)customerRepo.AttachMetadataToPutDto(x, customer)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = customerRepo.GetByIdForBrowserAsync(customer.Id).Result,
                        Message = ApiMessages.OK()
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = await z
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
        public async Task<Response> Delete([FromRoute] int id) {
            var x = await customerRepo.GetByIdAsync(id, false);
            if (x != null) {
                customerRepo.Delete(x);
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

        [HttpGet("[action]/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdForValidationAsync(int id) {
            var x = await customerRepo.GetByIdAsync(id, false);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.CustomerDataIsInvalid(),
                    Body = customerRepo.GetCustomerData(x)
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

    }

}