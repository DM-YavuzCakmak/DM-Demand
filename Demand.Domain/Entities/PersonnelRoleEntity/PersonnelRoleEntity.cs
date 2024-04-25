using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.PersonnelRole;

[Table("PersonnelRole")]
public class PersonnelRoleEntity : BaseEntity
{
    public long PersonnelId { get; set; }
    public long RoleId { get; set; }
}
