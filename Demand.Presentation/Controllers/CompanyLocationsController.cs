using Demand.Business.Abstract.CompanyLocation;
using Microsoft.AspNetCore.Mvc;

namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
