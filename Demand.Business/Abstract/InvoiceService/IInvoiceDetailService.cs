using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.InvoiceEntity;

namespace Demand.Business.Abstract.InvoiceService
{
    public interface IInvoiceDetailService
    {
        IDataResult<IList<InvoiceDetailEntity>> GetAll();
        IDataResult<InvoiceDetailEntity> GetById(long id);
        IDataResult<InvoiceDetailEntity> GetByUUID(Guid UUID);
        InvoiceDetailEntity Add(InvoiceDetailEntity invoiceDetailEntity);
        InvoiceDetailEntity Update(InvoiceDetailEntity invoiceDetailEntity);
    }
}
