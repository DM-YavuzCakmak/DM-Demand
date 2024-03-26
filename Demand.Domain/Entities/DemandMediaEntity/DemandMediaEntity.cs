using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demand.Domain.Entities.Demand;

namespace Demand.Domain.Entities.DemandMediaEntity
{

    [Table("DemandMedia")]
    public class DemandMediaEntity:BaseEntity
    {
 
        [Required]
        public long DemandId { get; set; }
        [MaxLength]
        public string Path { get; set; }

        public string? FileName{ get; set; }
        public long? DemandOfferId { get; set; }

        [ForeignKey("DemandId")]
        public virtual DemandEntity Demand { get; set; }
    }
}
