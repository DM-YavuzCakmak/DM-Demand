using Demand.Business.Abstract.DemandMediaService;
using Demand.Business.Abstract.DemandService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Infrastructure.DataAccess.Abstract.DemandMedia;
using Demand.Infrastructure.DataAccess.Abstract.IDemandRepository;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Demand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
