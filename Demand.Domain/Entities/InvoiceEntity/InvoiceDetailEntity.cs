using Demand.Domain.Entities.Personnel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.InvoiceEntity
{
    [Table("InvoiceDetail")]
    public class InvoiceDetailEntity : BaseEntity
    {
        public Guid InvoiceUUID { get; set; }
        public string? EInvoiceNumber { get; set; }
        public string? NebimInvoiceNumber { get; set; }
        public long? SentToDepartmentId { get; set; }
        public int InvoiceType { get; set; }
        public int Status { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? RejectionNote { get; set; }

        // Navigation properties (Personnel tablosu ile ilişkiler)

        [ForeignKey("SentToDepartmentId")]
        public virtual DepartmentEntity.DepartmentEntity? SentToDepartment { get; set; } = new DepartmentEntity.DepartmentEntity();


        [ForeignKey("CreatedAt")]
        public virtual PersonnelEntity? CreatedBy { get; set; } = new PersonnelEntity();

        [ForeignKey("UpdatedAt")]
        public virtual PersonnelEntity? UpdatedBy { get; set; } = new PersonnelEntity();
    }
}
