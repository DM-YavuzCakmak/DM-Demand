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

    [ForeignKey("ParentId")]
    public virtual PersonnelEntity Parent { get; set; }
}
