using Demand.Business.Abstract.CompanyService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Company;
using Demand.Infrastructure.DataAccess.Abstract.ICompanyRepository;

namespace Demand.Business.Concrete.CompanyService
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public IDataResult<Company> GetById(long id)
        {
            return new SuccessDataResult<Company>(_companyRepository.Get(x => x.Id == id));
        }

        public IDataResult<List<Company>> GetList()
        {
            return new SuccessDataResult<List<Company>>(_companyRepository.GetList().ToList());
        }
    }
}
