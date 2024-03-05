using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.Personnel;

namespace Demand.Domain.Entities.DemandProcess
{
    [Table("DemandProcess")]
    public class DemandProcessEntity : BaseEntity
    {
        [Required]
        public long DemandId { get; set; }

        [Required]
        public long ManagerId { get; set; }

        public string Desciription { get; set; }

        public bool? Status { get; set; }

        public int? HierarchyOrder { get; set; }

        [ForeignKey("DemandId")]
        public virtual DemandEntity Demand { get; set; }

        [ForeignKey("ManagerId")]
        public virtual PersonnelEntity Manager { get; set; }
    }
}
