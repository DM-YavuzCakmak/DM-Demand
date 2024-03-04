using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Company;

namespace Demand.Business.Abstract
{
    public interface ICompanyService
    {
        IDataResult<List<Company>> GetList();
    }
}
