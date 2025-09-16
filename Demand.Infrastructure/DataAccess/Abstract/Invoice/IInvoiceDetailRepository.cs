using Demand.Core.DataAccess.EntityFramework;

namespace Demand.Infrastructure.DataAccess.Abstract.Invoice
{
    public interface IInvoiceDetailRepository : IEntityRepository<Demand.Domain.Entities.InvoiceEntity.InvoiceDetailEntity>
    {
    }
}
