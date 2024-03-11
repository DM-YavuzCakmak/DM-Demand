using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.DemandMediaEntity;

namespace Demand.Business.Abstract.CompanyLocation
{
    public interface ICompanyLocationService
    {
        IDataResult<List<Demand.Domain.Entities.CompanyLocation.CompanyLocation>> GetList();
        IDataResult<Demand.Domain.Entities.CompanyLocation.CompanyLocation> GetById(long id);
        IDataResult <List<Demand.Domain.Entities.CompanyLocation.CompanyLocation>> GetLocationByCompanyId(long companyId);

        IDataResult<IList<Demand.Domain.Entities.CompanyLocation.CompanyLocation>> GetAll();
    }  

}
