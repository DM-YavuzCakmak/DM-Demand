using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.InvoiceEntity;
using Demand.Infrastructure.DataAccess.Abstract.Invoice;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Invoice
{
    public class InvoiceDemandRepository : EfEntityRepositoryBase<InvoiceDemandEntity, DemandContext>, IInvoiceDemandRepository
    {
    }
}
