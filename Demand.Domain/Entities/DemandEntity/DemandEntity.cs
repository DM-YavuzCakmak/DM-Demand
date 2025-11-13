using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.Demand;

[Table("Demand")]
public class DemandEntity : BaseEntity
{
    public string  DemandTitle{ get; set; }
    public long CompanyLocationId { get; set; }
    public long? CompanyId { get; set; }
    public long? LocationUnitId { get; set; }
    public long DepartmentId { get; set; }
    public int Status { get; set; }
    public string Description { get; set; }
    public DateTime RequirementDate { get; set; }
}
