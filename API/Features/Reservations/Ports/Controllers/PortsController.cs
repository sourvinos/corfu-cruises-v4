using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Ports {

    [Route("api/[controller]")]
    public class PortsController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IPortRepository portRepo;
        private readonly IPortValidation portValidation;

        #endregion

        public PortsController(IMapper mapper, IPortRepository portRepository, IPortValidation portValidation) {
            this.mapper = mapper;
            this.portRepo = portRepository;
            this.portValidation = portValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<PortListVM>> GetAsync() {
            return await portRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<PortBrowserVM>> GetForBrowserAsync() {
            return await portRepo.GetForBrowserAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            return await portRepo.GetForCriteriaAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await portRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Port, PortReadDto>(x)
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
        public Response Post([FromBody] PortWriteDto port) {
            var x = portValidation.IsValid(null, port);
            if (x == 200) {
                var z = portRepo.Create(mapper.Map<PortWriteDto, Port>((PortWriteDto)portRepo.AttachMetadataToPostDto(port)));
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
        public async Task<Response> Put([FromBody] PortWriteDto port) {
            var x = await portRepo.GetByIdAsync(port.Id);
            if (x != null) {
                var z = portValidation.IsValid(x, port);
                if (z == 200) {
                    portRepo.Update(mapper.Map<PortWriteDto, Port>((PortWriteDto)portRepo.AttachMetadataToPutDto(x, port)));
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
            var x = await portRepo.GetByIdAsync(id);
            if (x != null) {
                portRepo.Delete(x);
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