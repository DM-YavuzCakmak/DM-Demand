using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.CurrencyTypeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.CurrencyTypeService
{
    public interface ICurrencyTypeService
    {
        IDataResult<IList<CurrencyTypeEntity>> GetAll();
        CurrencyTypeEntity Add(CurrencyTypeEntity currencyType);
        IDataResult<CurrencyTypeEntity> GetById(long id);
        IDataResult<IList<CurrencyTypeEntity>> GetList(Expression<Func<CurrencyTypeEntity, bool>> filter);
    }
}
