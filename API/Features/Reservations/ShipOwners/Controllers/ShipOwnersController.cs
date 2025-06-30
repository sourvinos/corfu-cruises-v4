using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.ShipOwners {

    [Route("api/[controller]")]
    public class ShipOwnersController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IShipOwnerRepository shipOwnerRepo;
        private readonly IShipOwnerValidation shipOwnerValidation;

        #endregion

        public ShipOwnersController(IMapper mapper, IShipOwnerRepository shipOwnerRepo, IShipOwnerValidation shipOwnerValidation) {
            this.mapper = mapper;
            this.shipOwnerRepo = shipOwnerRepo;
            this.shipOwnerValidation = shipOwnerValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<ShipOwnerListVM>> GetAsync() {
            return await shipOwnerRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<ShipOwnerBrowserVM>> GetForBrowserAsync() {
            return await shipOwnerRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await shipOwnerRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<ShipOwner, ShipOwnerReadDto>(x)
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
        public async Task<Response> Post([FromBody] ShipOwnerWriteDto shipOwner) {
            var x = shipOwnerValidation.IsValidAsync(null, shipOwner);
            if (await x == 200) {
                var z = shipOwnerRepo.Create(mapper.Map<ShipOwnerWriteDto, ShipOwner>((ShipOwnerWriteDto)shipOwnerRepo.AttachMetadataToPostDto(shipOwner)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = z.Id.ToString(),
                    Message = ApiMessages.OK()
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
        public async Task<Response> Put([FromBody] ShipOwnerWriteDto shipOwner) {
            var x = await shipOwnerRepo.GetByIdAsync(shipOwner.Id);
            if (x != null) {
                var z = shipOwnerValidation.IsValidAsync(x, shipOwner);
                if (await z == 200) {
                    shipOwnerRepo.Update(mapper.Map<ShipOwnerWriteDto, ShipOwner>((ShipOwnerWriteDto)shipOwnerRepo.AttachMetadataToPutDto(x, shipOwner)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = x.Id.ToString(),
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
            var x = await shipOwnerRepo.GetByIdAsync(id);
            if (x != null) {
                shipOwnerRepo.Delete(x);
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