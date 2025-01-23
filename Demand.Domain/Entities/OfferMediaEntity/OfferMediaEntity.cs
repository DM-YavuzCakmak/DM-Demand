using Demand.Domain.Entities.Demand;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demand.Domain.Entities.DemandOfferEntity;

namespace Demand.Domain.Entities.OfferMediaEntity
{
    [Table("OfferMedia")]
    public class OfferMediaEntity : BaseEntity
    {
        [Required]
        public long OfferId { get; set; }
        [MaxLength]
        public string Path { get; set; }
        public string? FileName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? DemandId { get; set; }

        [ForeignKey("OfferId")]
        public virtual DemandOfferEntity.DemandOfferEntity DemandOffer { get; set; }
    }
}
