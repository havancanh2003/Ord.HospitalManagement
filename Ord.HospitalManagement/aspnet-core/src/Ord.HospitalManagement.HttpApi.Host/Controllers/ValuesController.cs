using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ord.HospitalManagement.DTOs.Hospital;
using Ord.HospitalManagement.TenantAppService;
using System.Threading.Tasks;

namespace Ord.HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //private readonly TenantHospitalAppService _hospitalAppService;

        public ValuesController(TenantHospitalAppService hospitalAppService)
        {
           // _hospitalAppService = hospitalAppService;
        }

        [HttpPost]
        public async Task<ActionResult<HospitalDto>> CreateAsync([FromBody] CreateUpdateHospitalDto input)
        {
            var a = new CreateUpdateHospitalDto();
            var b = new CreateUserHospital();

           // var result = await _hospitalAppService.CreateHospitalAsync(a,b);
            return Ok();
        }
    }
}
