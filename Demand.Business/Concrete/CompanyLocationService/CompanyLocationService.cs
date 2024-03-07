using Demand.Business.Abstract.CompanyLocation;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Infrastructure.DataAccess.Abstract.ICompanyLocationRepository;

namespace Demand.Business.Concrete.CompanyLocation
{
    public class CompanyLocationService : ICompanyLocationService
    {
        private readonly ICompanyLocationRepository _companyLocationRepository;
        public CompanyLocationService(ICompanyLocationRepository companyLocationRepository)
        {
            _companyLocationRepository = companyLocationRepository;
        }

        public IDataResult<Domain.Entities.CompanyLocation.CompanyLocation> GetById(long id)
        {
            return new SuccessDataResult<Demand.Domain.Entities.CompanyLocation.CompanyLocation>(_companyLocationRepository.Get(x => x.Id == id));
        }

        public IDataResult<List<Domain.Entities.CompanyLocation.CompanyLocation>> GetList()
        {
            return new SuccessDataResult<List<Demand.Domain.Entities.CompanyLocation.CompanyLocation>>(_companyLocationRepository.GetList().ToList());

        }
        public IDataResult<List<Domain.Entities.CompanyLocation.CompanyLocation>> GetLocationByCompanyId(long companyId)
        {
            var locations = _companyLocationRepository.GetList(x => x.CompanyId == companyId).ToList();

            return new SuccessDataResult<List<Domain.Entities.CompanyLocation.CompanyLocation>>(locations);
        }
    }
}
