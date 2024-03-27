using Demand.Business.Abstract.ApprovedSupplierService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.ApprovedSupplierEntity;
using Demand.Domain.Entities.Demand;
using Demand.Infrastructure.DataAccess.Abstract.ApprovedSuplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.ApprovedSupplierService
{
    public class ApprovedSupplierService : IApprovedSupplierService
    {
        private readonly IApprovedSupplierRepository _approvedSupplierRepository;
        public ApprovedSupplierService(IApprovedSupplierRepository approvedSupplierRepository)
        {
            _approvedSupplierRepository = approvedSupplierRepository;
        }

        public ApprovedSupplierEntity Add(ApprovedSupplierEntity approvedSupplier)
        {
            _approvedSupplierRepository.Add(approvedSupplier);
            return approvedSupplier;
        }

        public IDataResult<IList<ApprovedSupplierEntity>> GetAll()
        {
            return new SuccessDataResult<IList<ApprovedSupplierEntity>>(_approvedSupplierRepository.GetAll());
        }

        public IDataResult<ApprovedSupplierEntity> GetById(long id)
        {
            return new SuccessDataResult<ApprovedSupplierEntity>(_approvedSupplierRepository.Get(x => x.Id == id));
        }

        public IDataResult<IList<ApprovedSupplierEntity>> GetList(Expression<Func<ApprovedSupplierEntity, bool>> filter)
        {
            return new SuccessDataResult<IList<ApprovedSupplierEntity>>(_approvedSupplierRepository.GetList(filter));
        }

        public ApprovedSupplierEntity Update(ApprovedSupplierEntity approvedSupplier)
        {
            _approvedSupplierRepository.Update(approvedSupplier);
            return approvedSupplier;
        }
    }
}
