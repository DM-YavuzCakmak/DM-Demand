using Demand.Business.Abstract.Department;
using Demand.Business.Abstract.PersonnelService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Infrastructure.DataAccess.Abstract.Department;
using Demand.Infrastructure.DataAccess.Abstract.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public IDataResult<IList<DepartmentEntity>> GetAll()
        {
            return new SuccessDataResult<IList<DepartmentEntity>>(_departmentRepository.GetAll());
        }

        public IDataResult<DepartmentEntity> GetById(long id)
        {
            return new SuccessDataResult<DepartmentEntity>(_departmentRepository.Get(x => x.Id == id));
        }


    }
}
