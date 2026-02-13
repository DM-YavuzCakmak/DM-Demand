using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.Personnel;

[Table("Personnel")]
public class PersonnelEntity : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public long? ParentId { get; set; }
    public int? DepartmentId { get; set; }

    public bool? IsViewNewInvoice { get; set; } = false;
    public bool? IsViewControlInvoice { get; set; } = false;

    [ForeignKey("ParentId")]
    public virtual PersonnelEntity Parent { get; set; }
}
