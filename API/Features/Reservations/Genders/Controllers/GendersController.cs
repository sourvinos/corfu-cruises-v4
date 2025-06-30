using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Genders {

    [Route("api/[controller]")]
    public class GendersController : ControllerBase {

        #region variables

        private readonly IGenderRepository genderRepo;

        #endregion

        public GendersController(IGenderRepository genderRepo) {
            this.genderRepo = genderRepo;

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<GenderBrowserVM>> GetForBrowserAsync() {
            return await genderRepo.GetForBrowserAsync();
        }

    }

}