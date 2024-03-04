using Demand.Business.Abstract.CompanyService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Company;
using Demand.Infrastructure.DataAccess.Abstract;

namespace Demand.Business.Concrete.CompanyService
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public IDataResult<CompanyEntity> GetById(long id)
        {
            return new SuccessDataResult<CompanyEntity>(_companyRepository.Get(x => x.Id == id));
        }

        public IDataResult<List<CompanyEntity>> GetList()
        {
            return new SuccessDataResult<List<CompanyEntity>>(_companyRepository.GetList().ToList());
        }
    }
}
