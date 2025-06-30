using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.CoachRoutes {

    [Route("api/[controller]")]
    public class CoachRoutesController : ControllerBase {

        #region variables

        private readonly ICoachRouteRepository coachRouteRepo;
        private readonly ICoachRouteValidation coachRouteValidation;
        private readonly IMapper mapper;

        #endregion

        public CoachRoutesController(ICoachRouteRepository coachRouteRepo, ICoachRouteValidation coachRouteValidation, IMapper mapper) {
            this.coachRouteRepo = coachRouteRepo;
            this.coachRouteValidation = coachRouteValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<CoachRouteListVM>> GetAsync() {
            return await coachRouteRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<CoachRouteBrowserVM>> GetForBrowserAsync() {
            return await coachRouteRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await coachRouteRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<CoachRoute, CoachRouteReadDto>(x)
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
        public Response Post([FromBody] CoachRouteWriteDto coachRoute) {
            var x = coachRouteValidation.IsValid(null, coachRoute);
            if (x == 200) {
                var z = coachRouteRepo.Create(mapper.Map<CoachRouteWriteDto, CoachRoute>((CoachRouteWriteDto)coachRouteRepo.AttachMetadataToPostDto(coachRoute)));
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
        public async Task<Response> Put([FromBody] CoachRouteWriteDto coachRoute) {
            var x = await coachRouteRepo.GetByIdAsync(coachRoute.Id);
            if (x != null) {
                var z = coachRouteValidation.IsValid(x, coachRoute);
                if (z == 200) {
                    coachRouteRepo.Update(mapper.Map<CoachRouteWriteDto, CoachRoute>((CoachRouteWriteDto)coachRouteRepo.AttachMetadataToPutDto(x, coachRoute)));
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
            var x = await coachRouteRepo.GetByIdAsync(id);
            if (x != null) {
                coachRouteRepo.Delete(x);
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