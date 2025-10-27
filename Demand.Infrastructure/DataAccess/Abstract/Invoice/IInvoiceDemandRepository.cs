using Demand.Core.DataAccess.EntityFramework;

namespace Demand.Infrastructure.DataAccess.Abstract.Invoice
{
    public interface IInvoiceDemandRepository : IEntityRepository<Demand.Domain.Entities.InvoiceEntity.InvoiceDemandEntity>
    {
    }
}
