using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.CurrencyTypeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Infrastructure.DataAccess.Abstract.CurrencyType
{
    public interface ICurrencyTypeRepository: IEntityRepository<CurrencyTypeEntity>
    {
    }
}
