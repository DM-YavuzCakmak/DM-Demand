using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.InvoiceEntity
{
    [Table("InvoiceProcess")]
    public class InvoiceProcessEntity : BaseEntity
    {
        public long InvoiceDetailId { get; set; }
        public int ProcessType { get; set; }
    }
}
