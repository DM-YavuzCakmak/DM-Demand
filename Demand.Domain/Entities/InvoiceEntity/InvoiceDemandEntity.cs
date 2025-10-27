using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.InvoiceEntity
{
    [Table("InvoiceDemand")]
    public class InvoiceDemandEntity : BaseEntity
    {
        public long InvoiceId { get; set; }
        public long DemandId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
