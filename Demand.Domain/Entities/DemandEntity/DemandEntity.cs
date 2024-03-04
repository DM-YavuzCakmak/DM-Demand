using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.Demand;

[Table("Demand")]
public class DemandEntity : BaseEntity
{
    public long CompanyLocationId { get; set; }
    public long DepartmentId { get; set; }
    public byte Status { get; set; }
    public string Description { get; set; }
    public DateTime RequirementDate { get; set; }
}
