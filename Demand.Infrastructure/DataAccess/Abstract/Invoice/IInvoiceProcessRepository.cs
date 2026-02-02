using Demand.Core.DataAccess.EntityFramework;

namespace Demand.Infrastructure.DataAccess.Abstract.Invoice
{
    public interface IInvoiceProcessRepository : IEntityRepository<Demand.Domain.Entities.InvoiceEntity.InvoiceProcessEntity>
    {
    }
}
