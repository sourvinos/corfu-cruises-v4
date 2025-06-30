using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.DocumentTypes {

    [Route("api/[controller]")]
    public class DocumentTypesController : ControllerBase {

        #region variables

        private readonly IDocumentTypeRepository documentTypeRepo;
        private readonly IDocumentTypeValidation documentTypeValidation;
        private readonly IMapper mapper;

        #endregion

        public DocumentTypesController(IDocumentTypeRepository DocumentTypeRepo, IDocumentTypeValidation DocumentTypeValidation, IMapper mapper) {
            documentTypeRepo = DocumentTypeRepo;
            documentTypeValidation = DocumentTypeValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<DocumentTypeListVM>> GetAsync() {
            return await documentTypeRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<DocumentTypeBrowserVM>> GetForBrowserInvoiceAsync() {
            return await documentTypeRepo.GetForBrowserAsync(1);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<DocumentTypeBrowserVM>> GetForBrowserRetailAsync() {
            return await documentTypeRepo.GetForBrowserAsync(3);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<DocumentTypeBrowserVM>> GetForBrowserReceiptAsync() {
            return await documentTypeRepo.GetForBrowserAsync(2);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await documentTypeRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<DocumentType, DocumentTypeReadDto>(x)
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
        public async Task<ResponseWithBody> Post([FromBody] DocumentTypeWriteDto DocumentType) {
            var x = documentTypeValidation.IsValidAsync(null, DocumentType);
            if (await x == 200) {
                var z = documentTypeRepo.Create(mapper.Map<DocumentTypeWriteDto, DocumentType>((DocumentTypeWriteDto)documentTypeRepo.AttachMetadataToPostDto(DocumentType)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = documentTypeRepo.GetByIdForBrowserAsync(z.Id).Result,
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
        public async Task<ResponseWithBody> PutAsync([FromBody] DocumentTypeWriteDto documentType) {
            var x = await documentTypeRepo.GetByIdAsync(documentType.Id);
            if (x != null) {
                var z = documentTypeValidation.IsValidAsync(x, documentType);
                if (await z == 200) {
                    documentTypeRepo.Update(mapper.Map<DocumentTypeWriteDto, DocumentType>((DocumentTypeWriteDto)documentTypeRepo.AttachMetadataToPutDto(x, documentType)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = documentTypeRepo.GetByIdForBrowserAsync(documentType.Id).Result,
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
            var x = await documentTypeRepo.GetByIdAsync(id);
            if (x != null) {
                documentTypeRepo.Delete(x);
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

        [HttpGet("[action]/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetLastDocumentTypeNoAsync(int id) {
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Success.ToString(),
                Body = await documentTypeRepo.GetLastDocumentTypeNoAsync(id),
                Message = ApiMessages.OK()
            };
        }
 
    }

}