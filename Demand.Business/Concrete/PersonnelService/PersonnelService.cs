using Demand.Business.Abstract.PersonnelService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Personnel;
using Demand.Infrastructure.DataAccess.Abstract.IDemandRepository;
using Demand.Infrastructure.DataAccess.Abstract.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.PersonnelService
{
    public class PersonnelService : IPersonnelService
    {
        private readonly IPersonnelRepository _personnelRepository;

        public PersonnelService(IPersonnelRepository personnelRepository)
        {
            _personnelRepository = personnelRepository;
        }

        public List<PersonnelEntity> GetAllParentList(long id)
        {
            List<PersonnelEntity> personnelEntities = new List<PersonnelEntity>();

            PersonnelEntity mainPersonnelEntity = _personnelRepository.Get(x => x.Id == id);

            return personnelEntities;

        }

        public IDataResult<PersonnelEntity> GetById(long id)
        {
            return new SuccessDataResult<PersonnelEntity>(_personnelRepository.Get(x => x.Id == id));
        }

        public IDataResult<List<PersonnelEntity>> GetList(Expression<Func<PersonnelEntity, bool>> filter = null)
        {
            return new SuccessDataResult<List<PersonnelEntity>>(_personnelRepository.GetList(filter).ToList());
        }

        IDataResult<PersonnelEntity> IPersonnelService.GetByEmail(string email)
        {
            return new SuccessDataResult<PersonnelEntity>(_personnelRepository.Get(x => x.Email == email));
        }
    }
}
