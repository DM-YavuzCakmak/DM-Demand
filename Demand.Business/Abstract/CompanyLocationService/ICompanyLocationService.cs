using Demand.Core.Utilities.Results.Abstract;

namespace Demand.Business.Abstract.CompanyLocation
{
    public interface ICompanyLocationService
    {
        IDataResult<List<Demand.Domain.Entities.CompanyLocation.CompanyLocationEntity>> GetList();
        IDataResult<Demand.Domain.Entities.CompanyLocation.CompanyLocationEntity> GetById(long id);
    }
}
