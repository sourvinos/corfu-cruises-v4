using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Destinations {

    [Route("api/[controller]")]
    public class DestinationsController : ControllerBase {

        #region variables

        private readonly IDestinationRepository destinationRepo;
        private readonly IDestinationValidation destinationValidation;
        private readonly IMapper mapper;

        #endregion

        public DestinationsController(IDestinationRepository destinationRepo, IDestinationValidation destinationValidation, IMapper mapper) {
            this.destinationRepo = destinationRepo;
            this.destinationValidation = destinationValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<DestinationListVM>> GetAsync() {
            return await destinationRepo.GetAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<DestinationBrowserVM>> GetForBrowserAsync() {
            return await destinationRepo.GetForBrowserAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            return await destinationRepo.GetForCriteriaAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await destinationRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Destination, DestinationReadDto>(x)
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
        public Response Post([FromBody] DestinationWriteDto destination) {
            var x = destinationValidation.IsValid(null, destination);
            if (x == 200) {
                var z = destinationRepo.Create(mapper.Map<DestinationWriteDto, Destination>((DestinationWriteDto)destinationRepo.AttachMetadataToPostDto(destination)));
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
        public async Task<Response> Put([FromBody] DestinationWriteDto destination) {
            var x = await destinationRepo.GetByIdAsync(destination.Id);
            if (x != null) {
                var z = destinationValidation.IsValid(x, destination);
                if (z == 200) {
                    destinationRepo.Update(mapper.Map<DestinationWriteDto, Destination>((DestinationWriteDto)destinationRepo.AttachMetadataToPutDto(x, destination)));
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
            var x = await destinationRepo.GetByIdAsync(id);
            if (x != null) {
                destinationRepo.Delete(x);
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