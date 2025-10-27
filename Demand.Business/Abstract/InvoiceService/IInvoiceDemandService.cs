using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.InvoiceEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Abstract.InvoiceService
{
    public interface IInvoiceDemandService
    {
        IDataResult<IList<InvoiceDemandEntity>> GetAll();
        IDataResult<IList<InvoiceDemandEntity>> GetByInvoiceId(long invoiceId);
        IDataResult<InvoiceDemandEntity> GetById(long id);

        InvoiceDemandEntity Add(InvoiceDemandEntity InvoiceDemandEntity);
        InvoiceDemandEntity Update(InvoiceDemandEntity InvoiceDemandEntity);
        bool DeleteByInvoiceId(long invoiceId);
    }
}
