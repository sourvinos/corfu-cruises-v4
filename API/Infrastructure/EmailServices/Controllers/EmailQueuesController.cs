using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Infrastructure.EmailServices {

    [Route("api/[controller]")]
    public class EmailQueuesController : ControllerBase {

        #region variables

        private readonly IEmailQueueRepository emailQueueRepo;

        #endregion

        public EmailQueuesController(IEmailQueueRepository emailQueueRepo) {
            this.emailQueueRepo = emailQueueRepo;
        }

        [HttpPost]
        [AllowAnonymous]
        public ResponseWithBody Post([FromBody] EmailQueueDto emailQueue) {
            var z = emailQueueRepo.Create(emailQueueRepo.CreateEmailQueue(emailQueue));
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Success.ToString(),
                Body = z.EntityId,
                Message = ApiMessages.OK()
            };
        }

    }

}