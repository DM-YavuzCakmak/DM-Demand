using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.PersonnelService
{
    public interface IPersonnelService
    {
        IDataResult<List<PersonnelEntity>> GetList();
        IDataResult<PersonnelEntity> GetById(long id);
        IDataResult<PersonnelEntity> GetByEmail(string email);
    }
}
