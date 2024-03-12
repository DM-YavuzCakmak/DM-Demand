using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.CurrencyTypeEntity;
using Demand.Infrastructure.DataAccess.Abstract.CurrencyType;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.CurrencyType
{
    public class CurrencyTypeRepository:EfEntityRepositoryBase<CurrencyTypeEntity, DemandContext>, ICurrencyTypeRepository
    {
    }
}
