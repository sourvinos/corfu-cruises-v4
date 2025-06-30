using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Receipts {

    [Route("api/[controller]")]
    public class ReceiptsController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IReceiptCalculateBalanceRepo receiptCalculateBalanceRepo;
        private readonly IReceiptEmailSender emailSender;
        private readonly IReceiptPdfRepository receiptPdfRepo;
        private readonly IReceiptRepository receiptRepo;
        private readonly IReceiptValidation receiptValidation;

        #endregion

        public ReceiptsController(IMapper mapper, IReceiptCalculateBalanceRepo receiptCalculateBalanceRepo, IReceiptEmailSender emailSender, IReceiptPdfRepository receiptPdfRepo, IReceiptRepository transactionRepo, IReceiptValidation transactionValidation) {
            this.emailSender = emailSender;
            this.mapper = mapper;
            this.receiptCalculateBalanceRepo = receiptCalculateBalanceRepo;
            this.receiptPdfRepo = receiptPdfRepo;
            this.receiptRepo = transactionRepo;
            this.receiptValidation = transactionValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<ReceiptListVM>> GetAsync() {
            return await receiptRepo.GetAsync();
        }

        [HttpPost("{getForPeriod}")]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<ReceiptListVM>> GetForPeriodAsync([FromBody] ReceiptListCriteriaVM criteria) {
            return await receiptRepo.GetForPeriodAsync(criteria);
        }

        [HttpGet("{transactionId}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string transactionId) {
            var x = await receiptRepo.GetByIdAsync(transactionId, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Receipt, ReceiptReadDto>(x)
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
        public async Task<Response> PostAsync([FromBody] ReceiptWriteDto receipt) {
            receipt.InvoiceNo = await receiptRepo.IncreaseReceiptNoAsync(receipt);
            var x = receiptValidation.IsValidAsync(null, receipt);
            if (await x == 200) {
                receipt = receiptCalculateBalanceRepo.AttachBalancesToCreateDto(receipt, receiptCalculateBalanceRepo.CalculateBalances(receipt, receipt.CustomerId, receipt.ShipOwnerId));
                var z = receiptRepo.Create(mapper.Map<ReceiptWriteDto, Receipt>((ReceiptWriteDto)receiptRepo.AttachMetadataToPostDto(receipt)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = z.InvoiceId.ToString(),
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
        public async Task<ResponseWithBody> PutAsync([FromBody] ReceiptWriteDto receipt) {
            var x = await receiptRepo.GetByIdAsync(receipt.InvoiceId.ToString(), false);
            if (x != null) {
                var z = await receiptValidation.IsValidAsync(x, receipt);
                if (z == 200) {
                    receiptRepo.Update(mapper.Map<ReceiptWriteDto, Receipt>((ReceiptWriteDto)receiptRepo.AttachMetadataToPutDto(x, receipt)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = receipt.InvoiceId.ToString(),
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

        [HttpPost("buildReceiptPdfs")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> BuildReceiptPdfs([FromBody] string[] invoiceIds) {
            var filenames = new List<string>();
            foreach (var invoiceId in invoiceIds) {
                var x = await receiptRepo.GetByIdForPdfAsync(invoiceId);
                if (x != null) {
                    var z = receiptPdfRepo.BuildPdf(mapper.Map<Receipt, ReceiptPdfVM>(x));
                    filenames.Add(z);
                } else {
                    throw new CustomException() {
                        ResponseCode = 404
                    };
                }
            }
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK(),
                Body = filenames.ToArray()
            };
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "admin")]
        public async Task<Response> PatchInvoicesWithEmailPending([FromBody] string[] invoiceIds) {
            foreach (var invoiceId in invoiceIds) {
                var x = await receiptRepo.GetByIdForPatchEmailSent(invoiceId);
                if (x != null) {
                    receiptRepo.UpdateIsEmailPending(x, invoiceId);
                } else {
                    throw new CustomException() {
                        ResponseCode = 404
                    };
                }
            }
            return new Response {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK()
            };
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "admin")]
        public async Task<Response> PatchReceiptsWithEmailPending([FromBody] string[] invoiceIds) {
            foreach (var invoiceId in invoiceIds) {
                var x = await receiptRepo.GetByIdForPatchEmailSent(invoiceId);
                if (x != null) {
                    receiptRepo.UpdateIsEmailPending(x, invoiceId);
                } else {
                    throw new CustomException() {
                        ResponseCode = 404
                    };
                }
            }
            return new Response {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK()
            };
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "admin")]
        public async Task<Response> PatchReceiptsWithEmailSent([FromBody] string[] invoiceIds) {
            foreach (var invoiceId in invoiceIds) {
                var x = await receiptRepo.GetByIdForPatchEmailSent(invoiceId);
                if (x != null) {
                    receiptRepo.UpdateIsEmailSent(x, invoiceId);
                } else {
                    throw new CustomException() {
                        ResponseCode = 404
                    };
                }
            }
            return new Response {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK()
            };
        }

        [HttpPatch("isCancelled/{invoiceId}")]
        [Authorize(Roles = "admin")]
        public async Task<Response> PatchIsCancelled(string invoiceId) {
            var x = await receiptRepo.GetByIdAsync(invoiceId, false);
            if (x != null) {
                receiptRepo.UpdateIsCancelled(x, invoiceId);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = invoiceId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpGet("[action]/{filename}")]
        [Authorize(Roles = "admin")]
        public IActionResult OpenPdf([FromRoute] string filename) {
            return receiptPdfRepo.OpenPdf(filename);
        }

    }

}