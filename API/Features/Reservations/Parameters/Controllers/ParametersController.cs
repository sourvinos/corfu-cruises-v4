using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Parameters {

    [Route("api/[controller]")]
    public class ReservationParametersController : Controller {

        private readonly IMapper mapper;
        private readonly IReservationParametersRepository parametersRepo;
        private readonly IReservationParameterValidation parameterValidation;

        public ReservationParametersController(IMapper mapper, IReservationParametersRepository parametersRepo, IReservationParameterValidation parameterValidation) {
            this.mapper = mapper;
            this.parametersRepo = parametersRepo;
            this.parameterValidation = parameterValidation;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ResponseWithBody> Get() {
            var x = await parametersRepo.GetAsync();
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK(),
                Body = mapper.Map<ReservationParameter, ParameterReadDto>(x)
            };
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> Put([FromBody] ParameterWriteDto parameter) {
            var x = await parametersRepo.GetAsync();
            if (x != null) {
                var z = parameterValidation.IsValid(x, parameter);
                if (z == 200) {
                    parametersRepo.Update(mapper.Map<ParameterWriteDto, ReservationParameter>((ParameterWriteDto)parametersRepo.AttachMetadataToPutDto(x, parameter)));
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

    }

}