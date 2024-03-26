using Demand.Business.Abstract.PersonnelService;
using Demand.Business.Abstract.Provider;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.ProviderEntity;
using Demand.Infrastructure.DataAccess.Abstract.IDemandRepository;
using Demand.Infrastructure.DataAccess.Abstract.Provider;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Demand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.ProviderService
{
    public class ProviderService : IProviderService
    {

        private readonly IProviderRepository _providerRepository;
        public ProviderService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }
        public ProviderEntity Add(ProviderEntity provider)
        {
            _providerRepository.Add(provider);
            return provider;
        }

        public IDataResult<IList<ProviderEntity>> GetAll()
        {
            return new SuccessDataResult<IList<ProviderEntity>>(_providerRepository.GetAll());
        }

        public IDataResult<ProviderEntity> GetById(long id)
        {
            return new SuccessDataResult<ProviderEntity>(_providerRepository.Get(x => x.Id == id));
        }

        public IDataResult<IList<ProviderEntity>> GetList(Expression<Func<ProviderEntity, bool>> filter)
        {
            return new SuccessDataResult<IList<ProviderEntity>>(_providerRepository.GetList(filter));
        }

        public ProviderEntity Update(ProviderEntity provider)
        {
            _providerRepository.Update(provider);
            return provider;
        }
    }
}
