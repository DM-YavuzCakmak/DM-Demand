using Demand.Business.Abstract.CompanyLocation;
using Demand.Business.Abstract.CompanyLocationUnitsService;
using Demand.Business.Concrete.CompanyLocation;
using Demand.Core.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserToken]
    public class CompanyLocationUnitsController : ControllerBase
    {
        private readonly ICompanyLocationUnitsService _companyLocationUnitsService;

        public CompanyLocationUnitsController(ICompanyLocationUnitsService companyLocationUnitsService)
        {
            _companyLocationUnitsService = companyLocationUnitsService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_companyLocationUnitsService.GetList());
        }

        [HttpGet("{Id}")]
        public IActionResult GetById(long Id)
        {
            return Ok(_companyLocationUnitsService.GetById(Id));
        }
        [HttpGet("GetLocationUnits")]
        public ActionResult GetLocationUnits(int locationId)
        {
            return Ok(_companyLocationUnitsService.GetLocationUnitsByLocationId(locationId));
        }
    }
}
