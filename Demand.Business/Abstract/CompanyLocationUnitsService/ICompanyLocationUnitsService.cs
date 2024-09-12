using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.CompanyLocationUnitsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.CompanyLocationUnitsService
{
 
    public interface ICompanyLocationUnitsService
    {
        IDataResult<List<CompanyLocationUnitsEntity>> GetList();
        IDataResult<CompanyLocationUnitsEntity> GetById(long id);
        IDataResult<IList<CompanyLocationUnitsEntity>> GetAll();
        IDataResult<List<CompanyLocationUnitsEntity>> GetLocationUnitsByLocationId(long locationId);

    }
}
