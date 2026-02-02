using Demand.Business.Abstract.InvoiceService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.InvoiceEntity;
using Demand.Infrastructure.DataAccess.Abstract.Invoice;

namespace Demand.Business.Concrete.Invoice
{
    public class InvoiceProcessService : IInvoiceProcessService
    {
        private readonly IInvoiceProcessRepository _invoiceProcessRepository;
        public InvoiceProcessService(IInvoiceProcessRepository invoiceProcessRepository)
        {
            _invoiceProcessRepository = invoiceProcessRepository;
        }

        public IDataResult<IList<InvoiceProcessEntity>> GetAll()
        {
            return new SuccessDataResult<IList<InvoiceProcessEntity>>(_invoiceProcessRepository.GetAll());
        }

        public IDataResult<InvoiceProcessEntity> GetById(long id)
        {
            return new SuccessDataResult<InvoiceProcessEntity>(_invoiceProcessRepository.Get(x => x.Id == id));
        }

        public InvoiceProcessEntity Add(InvoiceProcessEntity invoiceProcessEntity)
        {
            _invoiceProcessRepository.Add(invoiceProcessEntity);
            return invoiceProcessEntity;
        }

        public InvoiceProcessEntity Update(InvoiceProcessEntity invoiceProcessEntity)
        {
            _invoiceProcessRepository.Update(invoiceProcessEntity);
            return invoiceProcessEntity;
        }
    }
}
