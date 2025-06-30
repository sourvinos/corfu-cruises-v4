using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Banks {

    [Route("api/[controller]")]
    public class BanksController : ControllerBase {

        #region variables

        private readonly IBankRepository bankRepo;
        private readonly IBankValidation bankValidation;
        private readonly IMapper mapper;

        #endregion

        public BanksController(IBankRepository bankRepo, IBankValidation bankValidation, IMapper mapper) {
            this.bankRepo = bankRepo;
            this.bankValidation = bankValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<BankListVM>> GetAsync() {
            return await bankRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<BankBrowserVM>> GetForBrowserAsync() {
            return await bankRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string id) {
            var x = await bankRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Bank, BankReadDto>(x)
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
        public Response Post([FromBody] BankWriteDto bank) {
            var x = bankValidation.IsValid(null, bank);
            if (x == 200) {
                var z = bankRepo.Create(mapper.Map<BankWriteDto, Bank>((BankWriteDto)bankRepo.AttachMetadataToPostDto(bank)));
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
        public async Task<Response> Put([FromBody] BankWriteDto bank) {
            var x = await bankRepo.GetByIdAsync(bank.Id.ToString());
            if (x != null) {
                var z = bankValidation.IsValid(x, bank);
                if (z == 200) {
                    bankRepo.Update(mapper.Map<BankWriteDto, Bank>((BankWriteDto)bankRepo.AttachMetadataToPutDto(x, bank)));
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
            var x = await bankRepo.GetByIdAsync(id);
            if (x != null) {
                bankRepo.Delete(x);
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