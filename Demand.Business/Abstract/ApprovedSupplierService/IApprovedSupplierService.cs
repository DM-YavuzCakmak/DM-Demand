using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.ApprovedSupplierEntity;
using System.Linq.Expressions;

namespace Demand.Business.Abstract.ApprovedSupplierService
{
    public interface IApprovedSupplierService
    {
        IDataResult<IList<ApprovedSupplierEntity>> GetAll();
        ApprovedSupplierEntity Add(ApprovedSupplierEntity approvedSupplier);
        ApprovedSupplierEntity Update(ApprovedSupplierEntity approvedSupplier);
        IDataResult<ApprovedSupplierEntity> GetById(long id);
        IDataResult<IList<ApprovedSupplierEntity>> GetList(Expression<Func<ApprovedSupplierEntity, bool>> filter);
    }
}
