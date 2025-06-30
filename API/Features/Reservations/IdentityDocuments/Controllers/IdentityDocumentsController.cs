using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.IdentityDocuments {

    [Route("api/[controller]")]
    public class IdentityDocumentsController : ControllerBase {

        #region variables

        private readonly IIdentityDocumentRepository identityDocumentRepo;
        private readonly IIdentityDocumentValidation identityDocumentValidation;
        private readonly IMapper mapper;

        #endregion

        public IdentityDocumentsController(IIdentityDocumentRepository identityDocumentRepository, IIdentityDocumentValidation identityDocumentValidation, IMapper mapper) {
            this.identityDocumentRepo = identityDocumentRepository;
            this.identityDocumentValidation = identityDocumentValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<IdentityDocumentListVM>> GetAsync() {
            return await identityDocumentRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<IdentityDocumentBrowserVM>> GetForBrowserAsync() {
            return await identityDocumentRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await identityDocumentRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<IdentityDocument, IdentityDocumentReadDto>(x)
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
        public Response Post([FromBody] IdentityDocumentWriteDto identityDocument) {
            var x = identityDocumentValidation.IsValid(null, identityDocument);
            if (x == 200) {
                var z = identityDocumentRepo.Create(mapper.Map<IdentityDocumentWriteDto, IdentityDocument>((IdentityDocumentWriteDto)identityDocumentRepo.AttachMetadataToPostDto(identityDocument)));
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
        public async Task<Response> Put([FromBody] IdentityDocumentWriteDto identityDocument) {
            var x = await identityDocumentRepo.GetByIdAsync(identityDocument.Id);
            if (x != null) {
                var z = identityDocumentValidation.IsValid(x, identityDocument);
                if (z == 200) {
                    identityDocumentRepo.Update(mapper.Map<IdentityDocumentWriteDto, IdentityDocument>((IdentityDocumentWriteDto)identityDocumentRepo.AttachMetadataToPutDto(x, identityDocument)));
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
            var x = await identityDocumentRepo.GetByIdAsync(id);
            if (x != null) {
                identityDocumentRepo.Delete(x);
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