using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.BankAccounts {

    [Route("api/[controller]")]
    public class BankAccountsController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IBankAccountRepository bankAccountRepo;
        private readonly IBankAccountValidation bankAccountValidation;

        #endregion

        public BankAccountsController(IMapper mapper, IBankAccountRepository bankAccountRepo, IBankAccountValidation bankAccountValidation) {
            this.mapper = mapper;
            this.bankAccountRepo = bankAccountRepo;
            this.bankAccountValidation = bankAccountValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<BankAccountListVM>> GetAsync() {
            return await bankAccountRepo.GetAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await bankAccountRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<BankAccount, BankAccountReadDto>(x),
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
        public Response Post([FromBody] BankAccountWriteDto bankAccount) {
            var x = bankAccountValidation.IsValid(null, bankAccount);
            if (x == 200) {
                var z = bankAccountRepo.Create(mapper.Map<BankAccountWriteDto, BankAccount>((BankAccountWriteDto)bankAccountRepo.AttachMetadataToPostDto(bankAccount)));
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
        public async Task<Response> Put([FromBody] BankAccountWriteDto bankAccount) {
            var x = await bankAccountRepo.GetByIdAsync(bankAccount.Id, false);
            if (x != null) {
                var z = bankAccountValidation.IsValid(x, bankAccount);
                if (z == 200) {
                    bankAccountRepo.Update(mapper.Map<BankAccountWriteDto, BankAccount>((BankAccountWriteDto)bankAccountRepo.AttachMetadataToPutDto(x, bankAccount)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = x.Id.ToString(),
                        Message = ApiMessages.OK(),
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
        public async Task<Response> Delete([FromRoute] int id) {
            var x = await bankAccountRepo.GetByIdAsync(id, false);
            if (x != null) {
                bankAccountRepo.Delete(x);
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