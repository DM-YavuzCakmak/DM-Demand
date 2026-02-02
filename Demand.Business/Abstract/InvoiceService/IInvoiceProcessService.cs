using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.InvoiceEntity;

namespace Demand.Business.Abstract.InvoiceService
{
    public interface IInvoiceProcessService
    {
        IDataResult<IList<InvoiceProcessEntity>> GetAll();
        IDataResult<InvoiceProcessEntity> GetById(long id);
        InvoiceProcessEntity Add(InvoiceProcessEntity invoiceProcessEntity);
        InvoiceProcessEntity Update(InvoiceProcessEntity invoiceProcessEntity);
    }
}
