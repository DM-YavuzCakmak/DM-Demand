using Demand.Core.Utilities.Results.Abstract;

namespace Demand.Business.Abstract.CompanyLocation
{
    public interface ICompanyLocationService
    {
        IDataResult<List<Demand.Domain.Entities.CompanyLocation.CompanyLocation>> GetList();
        IDataResult<Demand.Domain.Entities.CompanyLocation.CompanyLocation> GetById(long id);
        IDataResult <List<Demand.Domain.Entities.CompanyLocation.CompanyLocation>> GetLocationByCompanyId(long companyId);


    }
}
