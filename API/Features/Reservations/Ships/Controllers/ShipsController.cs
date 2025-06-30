using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Ships {

    [Route("api/[controller]")]
    public class ShipsController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IShipRepository shipRepo;
        private readonly IShipValidation shipValidation;

        #endregion

        public ShipsController(IMapper mapper, IShipRepository shipRepo, IShipValidation shipValidation) {
            this.mapper = mapper;
            this.shipRepo = shipRepo;
            this.shipValidation = shipValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<ShipListVM>> GetAsync() {
            return await shipRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<ShipBrowserVM>> GetForBrowserAsync() {
            return await shipRepo.GetForBrowserAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            return await shipRepo.GetForCriteriaAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await shipRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Ship, ShipReadDto>(x),
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
        public ResponseWithBody Post([FromBody] ShipWriteDto ship) {
            var x = shipValidation.IsValid(null, ship);
            if (x == 200) {
                var z = shipRepo.Create(mapper.Map<ShipWriteDto, Ship>((ShipWriteDto)shipRepo.AttachMetadataToPostDto(ship)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = shipRepo.GetByIdForBrowserAsync(z.Id).Result,
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
        public async Task<ResponseWithBody> Put([FromBody] ShipWriteDto ship) {
            var x = await shipRepo.GetByIdAsync(ship.Id, false);
            if (x != null) {
                var z = shipValidation.IsValid(x, ship);
                if (z == 200) {
                    shipRepo.Update(mapper.Map<ShipWriteDto, Ship>((ShipWriteDto)shipRepo.AttachMetadataToPutDto(x, ship)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = shipRepo.GetByIdForBrowserAsync(ship.Id).Result,
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
            var x = await shipRepo.GetByIdAsync(id, false);
            if (x != null) {
                shipRepo.Delete(x);
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