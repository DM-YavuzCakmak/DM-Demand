using Demand.Business.Abstract.RequestInfo;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.RequestInfoEntity;
using Demand.Infrastructure.DataAccess.Abstract.Personnel;
using Demand.Infrastructure.DataAccess.Abstract.RequestInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.RequestInfo
{
    public class RequestInfoService : IRequestInfoService
    {
        private readonly IRequestInfoRepository _requestInfoRepository;

        public RequestInfoService(IRequestInfoRepository requestInfoRepository)
        {
            _requestInfoRepository = requestInfoRepository;
        }

        public RequestInfoEntity Add(RequestInfoEntity requestInfo)
        {
            _requestInfoRepository.Add(requestInfo);
            return requestInfo;
        }

        public IDataResult<IList<RequestInfoEntity>> GetAll()
        {
            return new SuccessDataResult<IList<RequestInfoEntity>>(_requestInfoRepository.GetAll());
        }

        public IDataResult<RequestInfoEntity> GetByDemandId(long demandId)
        {
            return new SuccessDataResult<RequestInfoEntity>(_requestInfoRepository.GetList(x => x.DemandId == demandId).FirstOrDefault());
        }

        public IDataResult<RequestInfoEntity> GetById(long id)
        {
            return new SuccessDataResult<RequestInfoEntity>(_requestInfoRepository.Get(x => x.Id == id));
        }

        public IDataResult<IList<RequestInfoEntity>> GetList(Expression<Func<RequestInfoEntity, bool>> filter)
        {
            return new SuccessDataResult<IList<RequestInfoEntity>>(_requestInfoRepository.GetList(filter));
        }

        public RequestInfoEntity Update(RequestInfoEntity requestInfo)
        {
            _requestInfoRepository.Update(requestInfo);
            return requestInfo;
        }
    }
}