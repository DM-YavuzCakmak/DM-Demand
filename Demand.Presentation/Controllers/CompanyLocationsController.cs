using Demand.Business.Abstract.CompanyLocation;
using Demand.Core.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserToken]

    public class CompanyLocationsController : ControllerBase
    {
        private readonly ICompanyLocationService _companyLocationService;
        public CompanyLocationsController(ICompanyLocationService companyLocationService)
        {
            _companyLocationService = companyLocationService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_companyLocationService.GetList());
        }

        [HttpGet("{Id}")]
        public IActionResult GetById(long Id)
        {
            return Ok(_companyLocationService.GetById(Id));
        }

        [HttpGet("GetLocations")]
        public ActionResult GetLocations(int companyId)
        {
            return Ok(_companyLocationService.GetLocationByCompanyId(companyId));
        }
    }
}
