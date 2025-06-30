using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Drivers{

    [Route("api/[controller]")]
    public class DriversController : ControllerBase {

        #region variables

        private readonly IDriverRepository driverRepo;
        private readonly IDriverValidation driverValidation;
        private readonly IMapper mapper;

        #endregion

        public DriversController(IDriverRepository driverRepo, IDriverValidation driverValidation, IMapper mapper) {
            this.driverRepo = driverRepo;
            this.driverValidation = driverValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<DriverListVM>> GetAsync() {
            return await driverRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<DriverBrowserVM>> GetForBrowserAsync() {
            return await driverRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await driverRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Driver, DriverReadDto>(x)
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
        public Response Post([FromBody] DriverWriteDto driver) {
            var x = driverValidation.IsValid(null, driver);
            if (x == 200) {
                var z = driverRepo.Create(mapper.Map<DriverWriteDto, Driver>((DriverWriteDto)driverRepo.AttachMetadataToPostDto(driver)));
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
        public async Task<Response> Put([FromBody] DriverWriteDto driver) {
            var x = await driverRepo.GetByIdAsync(driver.Id);
            if (x != null) {
                var z = driverValidation.IsValid(x, driver);
                if (z == 200) {
                    driverRepo.Update(mapper.Map<DriverWriteDto, Driver>((DriverWriteDto)driverRepo.AttachMetadataToPutDto(x, driver)));
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
            var x = await driverRepo.GetByIdAsync(id);
            if (x != null) {
                driverRepo.Delete(x);
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