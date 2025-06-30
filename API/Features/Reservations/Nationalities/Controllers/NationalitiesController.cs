using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Nationalities {

    [Route("api/[controller]")]
    public class NationalitiesController : ControllerBase {

        #region variables

        private readonly INationalityRepository nationalityRepo;

        #endregion

        public NationalitiesController(INationalityRepository nationalityRepo) {
            this.nationalityRepo = nationalityRepo;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<NationalityBrowserVM>> GetForBrowserAsync() {
            return await nationalityRepo.GetForBrowserAsync();
        }

    }

}