using System.ComponentModel.DataAnnotations.Schema;
namespace Demand.Domain.Entities.CompanyLocation;

[Table("CompanyLocation")]
public class CompanyLocationEntity : BaseEntity
{
    public string Name { get; set; }
    public long CompanyId { get; set; }
}
