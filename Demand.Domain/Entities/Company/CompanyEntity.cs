using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.Company;

[Table("Company")]
public class CompanyEntity : BaseEntity
{
    public string Name { get; set; }
}
