using Demand.Business.Abstract.CompanyService;
using Microsoft.AspNetCore.Mvc;

namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_companyService.GetList());
        }

        [HttpGet("{Id}")]
        public IActionResult GetById(long Id)
        {
            return Ok(_companyService.GetById(Id));
        }
    }
}
