using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.TaxOffices {

    [Route("api/[controller]")]
    public class TaxOfficesController : ControllerBase {

        #region variables

        private readonly ITaxOfficeRepository taxOfficeRepo;
        private readonly ITaxOfficeValidation taxOfficeValidation;
        private readonly IMapper mapper;

        #endregion

        public TaxOfficesController(ITaxOfficeRepository taxOfficeRepo, ITaxOfficeValidation taxOfficeValidation, IMapper mapper) {
            this.taxOfficeRepo = taxOfficeRepo;
            this.taxOfficeValidation = taxOfficeValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<TaxOfficeListVM>> GetAsync() {
            return await taxOfficeRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<TaxOfficeBrowserVM>> GetForBrowserAsync() {
            return await taxOfficeRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await taxOfficeRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<TaxOffice, TaxOfficeReadDto>(x)
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
        public Response Post([FromBody] TaxOfficeWriteDto taxOffice) {
            var x = taxOfficeValidation.IsValid(null, taxOffice);
            if (x == 200) {
                var z = taxOfficeRepo.Create(mapper.Map<TaxOfficeWriteDto, TaxOffice>((TaxOfficeWriteDto)taxOfficeRepo.AttachMetadataToPostDto(taxOffice)));
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
        public async Task<Response> Put([FromBody] TaxOfficeWriteDto taxOffice) {
            var x = await taxOfficeRepo.GetByIdAsync(taxOffice.Id);
            if (x != null) {
                var z = taxOfficeValidation.IsValid(x, taxOffice);
                if (z == 200) {
                    taxOfficeRepo.Update(mapper.Map<TaxOfficeWriteDto, TaxOffice>((TaxOfficeWriteDto)taxOfficeRepo.AttachMetadataToPutDto(x, taxOffice)));
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
        public async Task<Response> Delete([FromRoute] int id) {
            var x = await taxOfficeRepo.GetByIdAsync(id);
            if (x != null) {
                taxOfficeRepo.Delete(x);
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