using Demand.Business.Abstract.InvoiceService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.InvoiceEntity;
using Demand.Infrastructure.DataAccess.Abstract.Invoice;

namespace Demand.Business.Concrete.Invoice
{
    public class InvoiceDemandService : IInvoiceDemandService
    {
        private readonly IInvoiceDemandRepository _InvoiceDemandRepository;
        public InvoiceDemandService(IInvoiceDemandRepository InvoiceDemandRepository)
        {
            _InvoiceDemandRepository = InvoiceDemandRepository;
        }

        public IDataResult<IList<InvoiceDemandEntity>> GetAll()
        {
            return new SuccessDataResult<IList<InvoiceDemandEntity>>(_InvoiceDemandRepository.GetList(x => x.IsDeleted == false));
        }

        public IDataResult<InvoiceDemandEntity> GetById(long id)
        {
            return new SuccessDataResult<InvoiceDemandEntity>(_InvoiceDemandRepository.Get(x => x.Id == id && x.IsDeleted == false));
        }

        public InvoiceDemandEntity Add(InvoiceDemandEntity InvoiceDemandEntity)
        {
            _InvoiceDemandRepository.Add(InvoiceDemandEntity);
            return InvoiceDemandEntity;
        }

        public InvoiceDemandEntity Update(InvoiceDemandEntity InvoiceDemandEntity)
        {
            _InvoiceDemandRepository.Update(InvoiceDemandEntity);
            return InvoiceDemandEntity;
        }

        public IDataResult<IList<InvoiceDemandEntity>> GetByInvoiceId(long invoiceId)
        {
            return new SuccessDataResult<IList<InvoiceDemandEntity>>(_InvoiceDemandRepository.GetList(x => x.InvoiceId == invoiceId && x.IsDeleted == false));
        }

        public bool DeleteByInvoiceId(long invoiceId)
        {
            var entities = _InvoiceDemandRepository.GetList(x => x.InvoiceId == invoiceId && x.IsDeleted == false);
            if (entities != null && entities.Any())
            {
                foreach (var entity in entities)
                {
                    entity.IsDeleted = true;
                    _InvoiceDemandRepository.Update(entity);
                }
            }
            return true;
        }
    }
}
