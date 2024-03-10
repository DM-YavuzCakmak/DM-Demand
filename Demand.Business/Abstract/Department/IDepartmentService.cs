using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Domain.Entities.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.Department
{
    public interface IDepartmentService
    {
        IDataResult<IList<DepartmentEntity>> GetAll();

        IDataResult<DepartmentEntity> GetById(long id);

    }
}
