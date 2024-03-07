using Demand.Business.Abstract.PersonnelService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Personnel;
using Demand.Infrastructure.DataAccess.Abstract.IDemandRepository;
using Demand.Infrastructure.DataAccess.Abstract.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.PersonnelService
{
    public class PersonnelService: IPersonnelService
    {
        private readonly IPersonnelRepository _personnelRepository;

        public PersonnelService(IPersonnelRepository personnelRepository)
        {
                _personnelRepository = personnelRepository;
        }
        public IDataResult<PersonnelEntity> GetById(long id)
        {
            return new SuccessDataResult<PersonnelEntity>(_personnelRepository.Get(x => x.Id == id));
        }

        public IDataResult<List<PersonnelEntity>> GetList()
        {
            return new SuccessDataResult<List<PersonnelEntity>>(_personnelRepository.GetList().ToList());
        }
    }
}
