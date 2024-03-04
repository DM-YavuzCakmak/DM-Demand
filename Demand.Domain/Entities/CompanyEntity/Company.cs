using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.Company;

[Table("Company")]
public class Company : BaseEntity
{
    public string Name { get; set; }
}
