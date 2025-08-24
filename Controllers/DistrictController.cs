
using Microsoft.AspNetCore.Mvc;
using Sufra.DTOs;
using Sufra.Services.IServices;

namespace Sufra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictServices _districtServices;
        public DistrictController(IDistrictServices districtServices)
        {
            _districtServices = districtServices;
        }

        //---------------------

        [HttpGet]
        public async Task<IActionResult> GetDistricts()
        {
            IEnumerable<DistrictDTO> districts = await _districtServices.GetAllAsync();
            return Ok(districts);
        }

    }
}
