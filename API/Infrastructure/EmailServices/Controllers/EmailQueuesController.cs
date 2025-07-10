using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Infrastructure.EmailServices {

    [Route("api/[controller]")]
    public class EmailQueuesController : ControllerBase {

        #region variables

        private readonly IEmailQueueRepository emailQueueRepo;
        private readonly IMapper mapper;

        #endregion

        public EmailQueuesController(IEmailQueueRepository emailQueueRepo, IMapper mapper) {
            this.emailQueueRepo = emailQueueRepo;
            this.mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        public ResponseWithBody Post([FromBody] EmailQueueDto emailQueue) {
            var z = emailQueueRepo.Create(mapper.Map<EmailQueueDto, EmailQueue>(emailQueue));
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Success.ToString(),
                Body = z.Id.ToString(),
                Message = ApiMessages.OK()
            };
        }

    }

}