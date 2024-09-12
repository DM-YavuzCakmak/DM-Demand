using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.Entities.CompanyLocationUnitsEntity
{
    [Table("CompanyLocationUnits")]
    public class CompanyLocationUnitsEntity: BaseEntity
    {
        public long LocationId { get; set; }
        public string  Name{ get; set; }
    }
}
