using Demand.Business.Abstract.InvoiceService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.InvoiceEntity;
using Demand.Infrastructure.DataAccess.Abstract.Invoice;

namespace Demand.Business.Concrete.Invoice
{
    public class InvoiceDetailService : IInvoiceDetailService
    {
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;
        public InvoiceDetailService(IInvoiceDetailRepository invoiceDetailRepository)
        {
            _invoiceDetailRepository = invoiceDetailRepository;
        }

        public IDataResult<IList<InvoiceDetailEntity>> GetAll()
        {
            return new SuccessDataResult<IList<InvoiceDetailEntity>>(_invoiceDetailRepository.GetList(x => x.IsDeleted == false));
        }

        public IDataResult<InvoiceDetailEntity> GetById(long id)
        {
            return new SuccessDataResult<InvoiceDetailEntity>(_invoiceDetailRepository.Get(x => x.Id == id));
        }

        public IDataResult<InvoiceDetailEntity> GetByUUID(Guid UUID)
        {
            return new SuccessDataResult<InvoiceDetailEntity>(_invoiceDetailRepository.Get(x => x.InvoiceUUID == UUID && x.IsDeleted == false));
        }

        public InvoiceDetailEntity Add(InvoiceDetailEntity invoiceDetailEntity)
        {
            _invoiceDetailRepository.Add(invoiceDetailEntity);
            return invoiceDetailEntity;
        }

        public InvoiceDetailEntity Update(InvoiceDetailEntity invoiceDetailEntity)
        {
            _invoiceDetailRepository.Update(invoiceDetailEntity);
            return invoiceDetailEntity;
        }
    }
}
