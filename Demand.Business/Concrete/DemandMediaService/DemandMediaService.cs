using Demand.Business.Abstract.DemandMediaService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Infrastructure.DataAccess.Abstract.DemandMedia;

namespace Demand.Business.Concrete.DemandMediaService
{
    public class DemandMediaService:IDemandMediaService
    {
        private readonly IDemandMediaRepository _demandMediaRepository;
        public DemandMediaService(IDemandMediaRepository demandMediaRepository)
        {
            _demandMediaRepository = demandMediaRepository;
        }

        public DemandMediaEntity AddDemandMedia(DemandMediaEntity demandMedia)
        {
            _demandMediaRepository.Add(demandMedia);
            return demandMedia;
        }

        public IDataResult<IList<DemandMediaEntity>> GetAll()
        {
            return new SuccessDataResult<IList<DemandMediaEntity>>(_demandMediaRepository.GetAll());
        }

        public IList<DemandMediaEntity> GetByDemandId(long id)
        {
            return _demandMediaRepository.GetList(x => x.DemandId == id);
        }
    }
}
