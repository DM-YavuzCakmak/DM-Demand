using Demand.Business.Abstract.CurrencyTypeService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.CurrencyTypeEntity;
using Demand.Infrastructure.DataAccess.Abstract.CurrencyType;
using System.Linq.Expressions;

namespace Demand.Business.Concrete.CurrencyTypeService
{
    public class CurrencyTypeService:ICurrencyTypeService
    {
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        public CurrencyTypeService(ICurrencyTypeRepository currencyTypeRepository)
        {
            _currencyTypeRepository = currencyTypeRepository;
        }

        public CurrencyTypeEntity Add(CurrencyTypeEntity currencyType)
        {
            _currencyTypeRepository.Add(currencyType);
            return (currencyType);
        }

        public IDataResult<IList<CurrencyTypeEntity>> GetAll()
        {
            return new SuccessDataResult<IList<CurrencyTypeEntity>>(_currencyTypeRepository.GetAll());
        }

        public IDataResult<CurrencyTypeEntity> GetById(long id)
        {
            return new SuccessDataResult<CurrencyTypeEntity>(_currencyTypeRepository.Get(x => x.Id == id));
        }

        public IDataResult<IList<CurrencyTypeEntity>> GetList(Expression<Func<CurrencyTypeEntity, bool>> filter)
        {
            return new SuccessDataResult<IList<CurrencyTypeEntity>>(_currencyTypeRepository.GetList(filter));
        }
    }
}
