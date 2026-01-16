using Demand.Domain.Entities.Personnel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.InvoiceEntity
{
    [Table("InvoiceDetail")]
    public class InvoiceDetailEntity : BaseEntity
    {
        public Guid InvoiceUUID { get; set; }
        public long ResponsiblePersonId { get; set; }
        public long? SentToDeparmentId { get; set; }
        public int InvoiceType { get; set; }
        public int Status { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? RejectionNote { get; set; }

        // Navigation properties (Personnel tablosu ile ilişkiler)

        [ForeignKey("CreatedAt")]
        public virtual PersonnelEntity? CreatedBy { get; set; } = new PersonnelEntity();

        [ForeignKey("ResponsiblePersonId")]
        public virtual PersonnelEntity? ResponsiblePerson { get; set; } = new PersonnelEntity();

        [ForeignKey("SentToUserId")]
        public virtual PersonnelEntity? SentToUser { get; set; } = new PersonnelEntity();

        [ForeignKey("UpdatedAt")]
        public virtual PersonnelEntity? UpdatedBy { get; set; } = new PersonnelEntity();
    }
}
