using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Prices {

    [Route("api/[controller]")]
    public class PricesController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IPriceRepository priceRepo;
        private readonly IPriceCloneRepository priceCloneRepo;
        private readonly IPriceValidation priceValidation;

        #endregion

        public PricesController(IMapper mapper, IPriceRepository priceRepo, IPriceCloneRepository priceCloneRepo, IPriceValidation priceValidation) {
            this.mapper = mapper;
            this.priceRepo = priceRepo;
            this.priceCloneRepo = priceCloneRepo;
            this.priceValidation = priceValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<PriceListVM>> GetAsync() {
            return await priceRepo.GetAsync();
        }

        [HttpPost("sales")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> Post([FromBody] SalesCriteriaVM criteria) {
            var x = await priceRepo.GetPricesForSalesAsync(criteria);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = x
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string id) {
            var x = await priceRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Price, PriceReadDto>(x)
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
        public Response Post([FromBody] PriceWriteDto price) {
            var x = priceValidation.IsValid(null, price);
            if (x == 200) {
                var z = priceRepo.Create(mapper.Map<PriceWriteDto, Price>((PriceWriteDto)priceRepo.AttachMetadataToPostDto(price)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = null,
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = x
                };
            }
        }

        [HttpPost("clonePrices")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> ClonePricesAsync([FromBody] PriceCloneCriteria criteria) {
            if (await ProcessCriteriaAsync(criteria)) {
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = null,
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 419
                };
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> Put([FromBody] PriceWriteDto price) {
            var x = await priceRepo.GetByIdAsync(price.Id.ToString(), false);
            if (x != null) {
                var z = priceValidation.IsValid(x, price);
                if (z == 200) {
                    priceRepo.Update(mapper.Map<PriceWriteDto, Price>((PriceWriteDto)priceRepo.AttachMetadataToPutDto(x, price)));
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
            var x = await priceRepo.GetByIdAsync(id, false);
            if (x != null) {
                priceRepo.Delete(x);
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

        [HttpDelete("deleteRange")]
        [Authorize(Roles = "admin")]
        public Response DeleteRange([FromBody] string[] ids) {
            priceRepo.DeleteRange(ids);
            return new Response {
                Code = 200,
                Icon = Icons.Success.ToString(),
                Id = null,
                Message = ApiMessages.OK()
            };
        }

        private async Task<bool> ProcessCriteriaAsync(PriceCloneCriteria criteria) {
            var recordsProcessed = 0;
            foreach (var customerId in criteria.CustomerIds) {
                foreach (var priceId in criteria.PriceIds) {
                    try {
                        var z = await priceRepo.GetByIdAsync(priceId, false);
                        var x = priceRepo.Create(mapper.Map<PriceWriteDto, Price>((PriceWriteDto)priceRepo.AttachMetadataToPostDto(priceCloneRepo.BuildPriceWriteDto(customerId, z))));
                        recordsProcessed++;
                    }
                    catch (System.Exception) {
                        break;
                    }
                }
            }
            return recordsProcessed == criteria.CustomerIds.Length * criteria.PriceIds.Length;
        }

    }

}