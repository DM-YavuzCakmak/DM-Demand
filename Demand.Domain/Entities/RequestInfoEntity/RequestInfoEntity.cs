using Demand.Domain.Entities.Demand;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.Entities.RequestInfoEntity
{
    [Table("RequestInfo")]
    public class RequestInfoEntity:BaseEntity
    {
        public long DemandId { get; set; }
        public string Metarial { get; set; }
        public int? Quantity { get; set; }
        public string Unit { get; set; }
    }
}
