using System.ComponentModel.DataAnnotations.Schema;
namespace Demand.Domain.Entities.CompanyLocation;

[Table("CompanyLocation")]
public class CompanyLocation : BaseEntity
{
    public string Name { get; set; }
    public long CompanyId { get; set; }
}
