using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Company;

namespace Demand.Business.Abstract.CompanyService
{
    public interface ICompanyService
    {
        IDataResult<List<CompanyEntity>> GetList();
        IDataResult<CompanyEntity> GetById(long id);
    }
}
