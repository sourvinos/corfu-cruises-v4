using API.Features.Reservations.Customers;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Features.Sales.Invoices {

    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase {

        #region variables

        private readonly ICustomerRepository customerRepo;
        private readonly IInvoiceCalculateBalanceRepo calculateBalanceRepo;
        private readonly IInvoiceReadRepository invoiceReadRepo;
        private readonly IInvoiceUpdateRepository invoiceUpdateRepo;
        private readonly IInvoiceValidation invoiceValidation;
        private readonly IMapper mapper;

        #endregion

        public InvoicesController(ICustomerRepository customerRepo, IInvoiceCalculateBalanceRepo calculateBalanceRepo, IInvoiceReadRepository invoiceReadRepo, IInvoiceUpdateRepository invoiceUpdateRepo, IInvoiceValidation invoiceValidation, IMapper mapper) {
            this.calculateBalanceRepo = calculateBalanceRepo;
            this.customerRepo = customerRepo;
            this.invoiceReadRepo = invoiceReadRepo;
            this.invoiceUpdateRepo = invoiceUpdateRepo;
            this.invoiceValidation = invoiceValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<InvoiceListVM>> GetAsync() {
            return await invoiceReadRepo.GetAsync();
        }

        [HttpPost("{getForPeriod}")]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<InvoiceListVM>> GetForPeriodAsync([FromBody] InvoiceListCriteriaVM criteria) {
            return await invoiceReadRepo.GetForPeriodAsync(criteria);
        }

        [HttpGet("{invoiceId}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string invoiceId) {
            var x = await invoiceReadRepo.GetByIdAsync(invoiceId, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Invoice, InvoiceReadDto>(x)
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
        public async Task<Response> PostAsync([FromBody] InvoiceCreateDto invoice) {
            invoice.InvoiceNo = await invoiceUpdateRepo.IncreaseInvoiceNoAsync(invoice);
            var x = invoiceValidation.IsValidAsync(null, invoice);
            if (await x == 200) {
                invoice.ShipOwnerId = await invoiceUpdateRepo.AttachShipOwnerIdToInvoiceAsync(invoice);
                invoice = calculateBalanceRepo.AttachBalancesToCreateDto(invoice, calculateBalanceRepo.CalculateBalances(invoice, invoice.CustomerId, invoice.ShipOwnerId));
                var z = invoiceUpdateRepo.Create(mapper.Map<InvoiceCreateDto, Invoice>((InvoiceCreateDto)invoiceUpdateRepo.AttachMetadataToPostDto(invoice)));
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
        public async Task<Response> PutAsync([FromBody] InvoiceUpdateDto invoice) {
            var x = await invoiceReadRepo.GetByIdAsync(invoice.InvoiceId.ToString(), false);
            if (x != null) {
                var z = invoiceValidation.IsValidAsync(x, invoice);
                if (await z == 200) {
                    var i = invoiceUpdateRepo.Update(invoice.InvoiceId, mapper.Map<InvoiceUpdateDto, Invoice>((InvoiceUpdateDto)invoiceUpdateRepo.AttachMetadataToPutDto(x, invoice)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = i.InvoiceId.ToString(),
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

        [HttpGet("validateCreditLimit/{customerId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> ValidateCreditLimit(int customerId) {
            var x = await customerRepo.GetByIdAsync(customerId, false);
            if (x != null) {
                var balanceLimit = x.BalanceLimit;
                var balance = calculateBalanceRepo.ValidateCreditLimit(customerId);
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Body = new InvoiceValidateBalanceVM {
                        Customer = new SimpleEntity { Id = x.Id, Description = x.Description },
                        BalanceLimit = balanceLimit,
                        ActualBalance = balance,
                        MaxAllowed = balanceLimit - balance
                    },
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPut("invoiceAade")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> PutInvoiceAade([FromBody] InvoiceAade invoiceAade) {
            var x = await invoiceReadRepo.GetInvoiceAadeByIdAsync(invoiceAade.InvoiceId.ToString());
            if (x != null) {
                invoiceUpdateRepo.UpdateInvoiceAade(invoiceAade);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = invoiceAade.InvoiceId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPut("invoiceOxygen")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> PutInvoiceOxygen([FromBody] InvoiceAade invoiceAade) {
            var x = await invoiceReadRepo.GetInvoiceAadeByIdAsync(invoiceAade.InvoiceId.ToString());
            if (x != null) {
                invoiceUpdateRepo.UpdateInvoiceOxygen(invoiceAade);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = invoiceAade.InvoiceId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "admin")]
        public async Task<Response> PatchInvoicesWithEmailPending([FromBody] string[] invoiceIds) {
            foreach (var invoiceId in invoiceIds) {
                var x = await invoiceReadRepo.GetByIdForPatchEmailSent(invoiceId);
                if (x != null) {
                    invoiceUpdateRepo.UpdateIsEmailPending(x, invoiceId);
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
        public async Task<Response> PatchInvoicesWithEmailSent([FromBody] string[] invoiceIds) {
            foreach (var invoiceId in invoiceIds) {
                var x = await invoiceReadRepo.GetByIdForPatchEmailSent(invoiceId);
                if (x != null) {
                    invoiceUpdateRepo.UpdateIsEmailSent(x, invoiceId);
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
            var x = await invoiceReadRepo.GetByIdAsync(invoiceId, false);
            if (x != null) {
                invoiceUpdateRepo.UpdateIsCancelled(x, invoiceId);
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

        [HttpDelete("{invoiceId}")]
        [Authorize(Roles = "admin")]
        public async Task<Response> Delete([FromRoute] string invoiceId) {
            var x = await invoiceReadRepo.GetByIdAsync(invoiceId, false);
            if (x != null) {
                invoiceUpdateRepo.Delete(x);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.InvoiceId.ToString(),
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