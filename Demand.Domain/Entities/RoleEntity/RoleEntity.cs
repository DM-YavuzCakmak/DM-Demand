using System.ComponentModel.DataAnnotations.Schema;
namespace Demand.Domain.Entities.Role;

[Table("Role")]
public class RoleEntity : BaseEntity
{
    public string Name { get; set; }
}
